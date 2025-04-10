using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using System.ComponentModel.DataAnnotations.Schema;
using static RS1_2024_25.API.Endpoints.CityEndpoints.AcademicYearGetAllEndpoint;
using static RS1_2024_25.API.Endpoints.CityEndpoints.SemesterGetAllEndpoint;


namespace RS1_2024_25.API.Endpoints.CityEndpoints;

//bez paging i bez filtera
[Route("academicYears")]
public class AcademicYearGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<AcademicYearGetAllResponse[]>
{
    [HttpGet("all")]
    public override async Task<AcademicYearGetAllResponse[]> HandleAsync( CancellationToken cancellationToken = default)
    {
        var result = await db.AcademicYears
                        .Select(s => new AcademicYearGetAllResponse
                        {
                            ID = s.ID,
                            Name = s.Description
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class AcademicYearGetAllResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}