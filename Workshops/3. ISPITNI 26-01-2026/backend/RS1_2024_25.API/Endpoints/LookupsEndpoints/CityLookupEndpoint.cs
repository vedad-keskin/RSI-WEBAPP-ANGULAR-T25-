using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.CityEndpoints.CityLookupEndpoint;

namespace RS1_2024_25.API.Endpoints.CityEndpoints;

[Route("cities")]
public class CityLookupEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<CityLookupRequest>
    .WithResult<CityLookupResponse[]>
{
    [HttpGet("lookup")]
    public override async Task<CityLookupResponse[]> HandleAsync(
        [FromQuery] CityLookupRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = db.Cities
            .AsQueryable();

        // Filter po RegionId
        if (request.RegionId.HasValue)
        {
            query = query.Where(c => c.RegionId == request.RegionId.Value);
        }

        // Filter po CountryId
        if (request.CountryId.HasValue)
        {
            query = query.Where(c => c.Region!.CountryId == request.CountryId.Value);
        }

        var result = await query
            .Select(c => new CityLookupResponse
            {
                ID = c.ID,
                Name = c.Name,
                RegionID = c.RegionId,
                RegionName = c.Region!.Name,
                CountryID = c.Region.CountryId,
                CountryName = c.Region.Country!.Name
            })
            .ToArrayAsync(cancellationToken);

        return result;
    }

    // DTO za zahtjev
    public class CityLookupRequest
    {
        public int? RegionId { get; set; } // Opcionalni filter za RegionId
        public int? CountryId { get; set; } // Opcionalni filter za CountryId
    }

    // DTO za odgovor
    public class CityLookupResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required int RegionID { get; set; }
        public required string RegionName { get; set; }
        public required int CountryID { get; set; }
        public required string CountryName { get; set; }
    }
}
