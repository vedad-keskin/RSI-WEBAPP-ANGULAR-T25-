using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.SemesterEndpoints.SemesterGetByStudentIdEndpoint;

namespace RS1_2024_25.API.Endpoints.SemesterEndpoints
{
    // kopirano iz cities1getallendpoint
    [Route("semesters")]
    public class SemesterGetByStudentIdEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
        .WithRequest<int>
        .WithResult<SemesterGetByStudentIdResponse[]>
    {
        [HttpGet("{id}")]
        public override async Task<SemesterGetByStudentIdResponse[]> HandleAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await db.Semesters
                .Include(x=> x.AcademicYear)
                .Include(x=> x.RecordedBy)
                .Where(x=> x.StudentId == id)
                            .Select(c => new SemesterGetByStudentIdResponse
                            {

                                ID = c.ID,
                                AcademicYearDescription = c.AcademicYear.Description,
                                YearOfStudy = c.YearOfStudy,
                                Renewal = c.Renewal,
                                Date = c.Date,
                                RecordedByName = c.RecordedBy.Email

                                
                            })
                            .ToArrayAsync(cancellationToken);

            return result;
        }

        public class SemesterGetByStudentIdResponse
        {
            public int ID { get; set; }
            public string AcademicYearDescription { get; set; }
            public int YearOfStudy { get; set; }
            public bool Renewal { get; set; }

            public DateTime Date { get; set; }
            public string RecordedByName { get; set; }
        }
    }
}
