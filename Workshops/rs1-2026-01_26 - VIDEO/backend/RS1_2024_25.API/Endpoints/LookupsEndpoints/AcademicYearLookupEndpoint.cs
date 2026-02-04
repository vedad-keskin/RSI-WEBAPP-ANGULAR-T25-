using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.LookupsEndpoints.AcademicYearLookupEndpoint;

namespace RS1_2024_25.API.Endpoints.LookupsEndpoints;

[Route("academic-years")]
public class AcademicYearLookupEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<AcademicYearLookupResponse[]>
{
    [HttpGet("lookup")]
    public override async Task<AcademicYearLookupResponse[]> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await db.AcademicYears
                        .OrderByDescending(a => a.StartDate)
                        .Select(a => new AcademicYearLookupResponse
                        {
                            ID = a.ID,
                            Description = a.Description
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class AcademicYearLookupResponse
    {
        public required int ID { get; set; }
        public required string Description { get; set; }
    }
}
