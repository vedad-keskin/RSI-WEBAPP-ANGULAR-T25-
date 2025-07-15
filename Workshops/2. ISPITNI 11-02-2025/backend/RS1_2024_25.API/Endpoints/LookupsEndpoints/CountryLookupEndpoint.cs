using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.LookupsEndpoints.CountryLookupEndpoint;

namespace RS1_2024_25.API.Endpoints.LookupsEndpoints;

// Endpoint bez paging-a i bez filtera
[Route("countries")]
public class CountryLookupEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<CountryLookupResponse[]>
{
    [HttpGet("lookup")]
    public override async Task<CountryLookupResponse[]> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await db.Countries
                        .Select(c => new CountryLookupResponse
                        {
                            ID = c.ID,
                            Name = c.Name
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class CountryLookupResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
    }
}
