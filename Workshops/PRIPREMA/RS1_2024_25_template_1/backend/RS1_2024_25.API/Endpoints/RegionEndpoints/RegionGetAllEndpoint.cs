using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.RegionEndpoints.RegionGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.RegionEndpoints;

// GET endpoint za vraćanje svih regija sa opcionalnim filterom po državi
[Route("regions")]
public class RegionGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<RegionGetAllRequest>
    .WithResult<RegionGetAllResponse[]>
{
    [HttpGet("all")]
    public override async Task<RegionGetAllResponse[]> HandleAsync(
        [FromQuery] RegionGetAllRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = db.Regions.AsQueryable();

        // Dodaj filter ako je proslijeđen CountryId
        if (request.CountryId.HasValue)
        {
            query = query.Where(r => r.CountryId == request.CountryId.Value);
        }

        var result = await query
            .Select(r => new RegionGetAllResponse
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
    public class RegionGetAllRequest
    {
        public int? CountryId { get; set; } // Opcionalni filter za ID države
    }

    // DTO za odgovor
    public class RegionGetAllResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required int CountryID { get; set; }
        public required string CountryName { get; set; }
    }
}
