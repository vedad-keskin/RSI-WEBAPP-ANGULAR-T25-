using global::RS1_2024_25.API.Data;
using global::RS1_2024_25.API.Helper.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Helper;
using static RS1_2024_25.API.Endpoints.ChatEndpoints.ChatUserGetEndpoint;

namespace RS1_2024_25.API.Endpoints.ChatEndpoints;

[Route("chat-users")]
public class ChatUserGetEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<ChatUserGetRequest>
    .WithResult<ChatUserGetResponse[]>
{
    [HttpGet]
    public override async Task<ChatUserGetResponse[]> HandleAsync([FromQuery] ChatUserGetRequest request, CancellationToken cancellationToken = default)
    {
        // Filtriranje osnovno na tip korisnika
        IQueryable<UserQueryResult> query;

        if (request.UserType == "professor")
        {
            query = db.Professors
                .Include(p => p.User)
                .Select(p => new UserQueryResult
                {
                    ID = p.UserId,
                    FirstName = p.User.FirstName,
                    LastName = p.User.LastName,
                    Email = p.User.Email,
                    Type = "Professor"
                });
        }
        else if (request.UserType == "student")
        {
            query = db.Students
                .Include(s => s.User)
                .Select(s => new UserQueryResult
                {
                    ID = s.UserId,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Email = s.User.Email,
                    Type = "Student"
                });
        }
        else
        {
            // Ako nije naveden filter za tip korisnika, dohvatiti sve korisnike
            query = db.MyAppUsers.IgnoreQueryFilters().Select(u => new UserQueryResult
            {
                ID = u.ID,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Type = u.IsAdmin ? "Admin" : u.IsDean ? "Dean" : "User"
            });
        }

        // Dodatni filter prema imenu
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(u =>
                (u.FirstName + " " + u.LastName).Contains(request.SearchTerm) ||
                (u.LastName + " " + u.FirstName).Contains(request.SearchTerm) ||
                u.Email.Contains(request.SearchTerm));
        }


        var result = await query
            .ToArrayAsync(cancellationToken);

        return result;
    }

    public class ChatUserGetRequest
    {
        public string? UserType { get; set; } // "professor", "student", null za sve korisnike
        public string? SearchTerm { get; set; } // Pretraga po imenu, prezimenu ili emailu
    }

    public class ChatUserGetResponse
    {
        public required int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Type { get; set; } // "Professor", "Student", itd.
    }

    private class UserQueryResult : ChatUserGetResponse { }
}
