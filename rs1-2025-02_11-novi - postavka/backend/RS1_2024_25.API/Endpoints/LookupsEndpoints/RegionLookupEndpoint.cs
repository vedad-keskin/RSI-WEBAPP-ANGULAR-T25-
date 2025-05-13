using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.LookupsEndpoints.RegionLookupEndpoint;

namespace RS1_2024_25.API.Endpoints.LookupsEndpoints;

// GET endpoint za vraćanje svih regija sa opcionalnim filterom po državi
[Route("regions")]
public class RegionLookupEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<RegionLookupRequest>
    .WithResult<RegionLookupResponse[]>
{
    [HttpGet("lookup")]
    public override async Task<RegionLookupResponse[]> HandleAsync(
        [FromQuery] RegionLookupRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = db.Regions.AsQueryable();

        // Dodaj filter ako je proslijeđen CountryId
        if (request.CountryId.HasValue)
        {
            query = query.Where(r => r.CountryId == request.CountryId.Value);
        }

        var result = await query
            .Select(r => new RegionLookupResponse
            {
                ID = r.ID,
                Name = r.Name,
                CountryID = r.CountryId,
                CountryName = r.Country!.Name
            })
            .ToArrayAsync(cancellationToken);

        return result;
    }

    // DTO za zahtjev
    public class RegionLookupRequest
    {
        public int? CountryId { get; set; } // Opcionalni filter za ID države
    }

    // DTO za odgovor
    public class RegionLookupResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required int CountryID { get; set; }
        public required string CountryName { get; set; }
    }
}
