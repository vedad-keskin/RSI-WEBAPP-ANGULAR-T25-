using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using System.ComponentModel.DataAnnotations.Schema;
using static RS1_2024_25.API.Endpoints.CityEndpoints.SemesterGetAllEndpoint;


namespace RS1_2024_25.API.Endpoints.CityEndpoints;

//bez paging i bez filtera
[Route("semesters")]
public class SemesterGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithResult<SemesterGetAllResponse[]>
{
    [HttpGet("{id}")]
    public override async Task<SemesterGetAllResponse[]> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await db.Semesters
                        .Include(x=> x.AcademicYear)
                        .Include(x=> x.RecordedBy)
                        .Where(x=> x.StudentId == id)  
                        .Select(s => new SemesterGetAllResponse
                        {
                            ID = s.ID,
                            AcademicYearId = s.AcademicYearId,
                            AcademicYear = s.AcademicYear,
                            RecordedById = s.RecordedById,
                            RecordedBy = s.RecordedBy,
                            Date = s.Date,
                            Price = s.Price,    
                            Renewal = s.Renewal,
                            YearOfStudy = s.YearOfStudy,
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class SemesterGetAllResponse
    {
        public int ID { get; set; }

        public int AcademicYearId { get; set; } // FK na korisnika
        public AcademicYear AcademicYear { get; set; } // Referenca na korisnički entitet


        public int? RecordedById { get; set; } // FK na korisnika
        public MyAppUser? RecordedBy { get; set; } // Referenca na korisnički entitet

        public int YearOfStudy { get; set; }
        public bool Renewal { get; set; }

        public DateTime Date { get; set; }

        public int Price { get; set; }
    }
}