using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using System.Threading;
using System.Threading.Tasks;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.StudentGetByIdEndpoint;

namespace RS1_2024_25.API.Endpoints.StudentEndpoints;

[Route("students")]
[MyAuthorization(isAdmin: true, isManager: false)]
public class StudentGetByIdEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithResult<StudentGetByIdResponse>
{
    [HttpGet("{id}")]
    public override async Task<StudentGetByIdResponse> HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var student = await db.Students
            .Where(s => s.ID == id && !s.IsDeleted)
            .Select(s => new StudentGetByIdResponse
            {
                ID = s.ID,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                StudentNumber = s.StudentNumber,
                ParentName = s.ParentName,
                BirthDate = s.BirthDate,
                Gender = s.Gender.ToString(),
                CitizenshipId = s.CitizenshipId,
                BirthPlace = s.BirthPlace,
                BirthMunicipalityId = s.BirthMunicipalityId,
                PermanentAddressStreet = s.PermanentAddressStreet,
                PermanentMunicipalityId = s.PermanentMunicipalityId,
                ContactMobilePhone = s.ContactMobilePhone,
                ContactPrivateEmail = s.ContactPrivateEmail
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (student == null)
        {
            throw new KeyNotFoundException("Student not found");
        }

        return student;
    }

    // DTO za odgovor
    public class StudentGetByIdResponse
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string StudentNumber { get; set; } = string.Empty;
        public string? ParentName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int? CitizenshipId { get; set; }
        public string? BirthPlace { get; set; }
        public int? BirthMunicipalityId { get; set; }
        public string? PermanentAddressStreet { get; set; }
        public int? PermanentMunicipalityId { get; set; }
        public string? ContactMobilePhone { get; set; }
        public string? ContactPrivateEmail { get; set; }
    }
}
