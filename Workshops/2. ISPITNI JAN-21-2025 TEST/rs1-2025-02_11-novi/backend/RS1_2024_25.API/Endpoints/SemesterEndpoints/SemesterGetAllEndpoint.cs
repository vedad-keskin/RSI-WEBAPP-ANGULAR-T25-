using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.SemesterGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.StudentEndpoints;


[Route("semesters")]
public class SemesterGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithResult<SemestersGetAllResponse[]>
{
    [HttpGet("{id}")]
    public override async Task<SemestersGetAllResponse[]> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await db.Semesters
                        .Include(x => x.AcademicYear)
                        .Include(x => x.RecordedBy)
                        .Where(x => x.StudentId == id)
                        .Select(s => new SemestersGetAllResponse
                        {
                            ID = s.ID,
                            AcademicYearDescription = s.AcademicYear.Description,
                            RecordedByName = s.RecordedBy.FirstName,
                            Date = s.Date,
                            Price = s.Price,
                            Renewal = s.Renewal,
                            YearOfStudy = s.YearOfStudy,
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class SemestersGetAllResponse
    {
        public int ID { get; set; }
        public string? AcademicYearDescription { get; set; } // Referenca na korisnički entitet
        public string? RecordedByName { get; set; } // Referenca na korisnički entitet
        public int YearOfStudy { get; set; }
        public bool Renewal { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
    }
}