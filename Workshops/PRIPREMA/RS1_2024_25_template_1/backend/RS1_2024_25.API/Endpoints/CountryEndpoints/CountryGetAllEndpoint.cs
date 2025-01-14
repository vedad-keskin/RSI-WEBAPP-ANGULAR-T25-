using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.CountryEndpoints.CountryGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.CountryEndpoints;

// Endpoint bez paging-a i bez filtera
[Route("countries")]
public class CountryGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithoutRequest
    .WithResult<CountryGetAllResponse[]>
{
    [HttpGet("all")]
    public override async Task<CountryGetAllResponse[]> HandleAsync(CancellationToken cancellationToken = default)
    {
        var result = await db.Countries
                        .Select(c => new CountryGetAllResponse
                        {
                            ID = c.ID,
                            Name = c.Name
                        })
                        .ToArrayAsync(cancellationToken);

        return result;
    }

    public class CountryGetAllResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
    }
}
