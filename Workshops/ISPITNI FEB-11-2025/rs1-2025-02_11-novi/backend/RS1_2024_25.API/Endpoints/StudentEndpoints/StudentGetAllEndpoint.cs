using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.StudentGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.StudentEndpoints;

// Endpoint za vraćanje liste studenata s filtriranjem i paginacijom
[Route("students")]
[MyAuthorization(isAdmin: true, isManager: false)]
public class StudentGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<StudentGetAllRequest>
    .WithResult<MyPagedList<StudentGetAllResponse>>
{
    [HttpGet("filter")]
    public override async Task<MyPagedList<StudentGetAllResponse>> HandleAsync([FromQuery] StudentGetAllRequest request, CancellationToken cancellationToken = default)
    {
        // Osnovni upit za studente
        var query = db.Students
                   .Where(s => !s.IsDeleted)
                   .AsQueryable();

        // Primjena filtera po imenu, prezimenu, student broju ili državi
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            query = query.Where(s =>
                s.User.FirstName.Contains(request.Q) ||
                s.User.LastName.Contains(request.Q) ||
                s.StudentNumber.Contains(request.Q) ||
                (s.Citizenship != null && s.Citizenship.Name.Contains(request.Q))
            );
        }

        // Projektovanje u DTO tip za rezultat
        var projectedQuery = query.Select(s => new StudentGetAllResponse
        {
            ID = s.ID,
            FirstName = s.User.FirstName,
            LastName = s.User.LastName,
            StudentNumber = s.StudentNumber,
            Citizenship = s.Citizenship != null ? s.Citizenship.Name : null,
            BirthMunicipality = s.BirthMunicipality != null ? s.BirthMunicipality.Name : null,
        });

        // Kreiranje paginiranog rezultata
        var result = await MyPagedList<StudentGetAllResponse>.CreateAsync(projectedQuery, request, cancellationToken);

        return result;
    }

    // DTO za zahtjev s podrškom za paginaciju i filtriranje
    public class StudentGetAllRequest : MyPagedRequest
    {
        public string? Q { get; set; } = string.Empty; // Tekstualni upit za pretragu
    }

    // DTO za odgovor
    public class StudentGetAllResponse
    {
        public required int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string StudentNumber { get; set; }
        public string? Citizenship { get; set; }
        public string? BirthMunicipality { get; set; }
    }
}
