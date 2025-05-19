using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.AcademicYearEndpoints.AcademicYearGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.AcademicYearEndpoints;


[Route("academicYear")]
public class AcademicYearGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<AcademicYearGetAllResponse[]>
{
    [HttpGet("all")]
    public override async Task<AcademicYearGetAllResponse[]> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await db.AcademicYears
                        .Select(c => new AcademicYearGetAllResponse
                        {
                            ID = c.ID,
                            Name = c.Description
                           
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class AcademicYearGetAllResponse
    {
        public required int ID { get; set; }
        public string Name { get; set; }

    }
}