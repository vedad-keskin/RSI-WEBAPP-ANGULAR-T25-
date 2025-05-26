using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.SemesterEndpoints.SemesterGetAllByStudentIdEndpoint;

namespace RS1_2024_25.API.Endpoints.SemesterEndpoints;


//bez paging i bez filtera
[Route("semesters")]
public class SemesterGetAllByStudentIdEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithResult<SemesterGetAllByStudentIdResponse[]>
{
    [HttpGet("{id}")]
    public override async Task<SemesterGetAllByStudentIdResponse[]> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await db.Semesters
            .Where(x=> x.StudentId == id)
            .Include(x=> x.AcademicYear)
            .Include(x=> x.RecordedBy)
                        .Select(c => new SemesterGetAllByStudentIdResponse
                        {
                            ID = c.ID,
                            AcademicYearDescription = $"{c.AcademicYear!.StartDate.Year}-{c.AcademicYear.EndDate.Year % 100}",
                            YearOfStudy = c.YearOfStudy,
                            Renewal = c.Renewal,
                            DateOfEnrollment = c.DateOfEnrollment,
                            RecordedByName = c.RecordedBy!.FirstName,
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class SemesterGetAllByStudentIdResponse
    {
        public required int ID { get; set; }
        public string? AcademicYearDescription { get; set; }
        public int YearOfStudy { get; set; }
        public bool Renewal { get; set; }
        public DateTime DateOfEnrollment { get; set; }
        public string? RecordedByName { get; set; }


    }
}
