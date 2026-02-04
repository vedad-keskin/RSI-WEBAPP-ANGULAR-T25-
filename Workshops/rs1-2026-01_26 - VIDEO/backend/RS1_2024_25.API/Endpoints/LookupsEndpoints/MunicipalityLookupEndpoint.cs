using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.MunicipalityEndpoints.MunicipalityLookupEndpoint;

namespace RS1_2024_25.API.Endpoints.MunicipalityEndpoints;

[Route("municipalities")]
public class MunicipalityLookupEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<MunicipalityLookupRequest>
    .WithResult<MunicipalityLookupResponse[]>
{
    [HttpGet("lookup")]
    public override async Task<MunicipalityLookupResponse[]> HandleAsync(
        [FromQuery] MunicipalityLookupRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = db.Municipalities
            .AsQueryable();

        // Filter po CityId
        if (request.CityId.HasValue)
        {
            query = query.Where(m => m.CityId == request.CityId.Value);
        }

        // Filter po RegionId
        if (request.RegionId.HasValue)
        {
            query = query.Where(m => m.City!.RegionId == request.RegionId.Value);
        }

        // Filter po CountryId
        if (request.CountryId.HasValue)
        {
            query = query.Where(m => m.City!.Region!.CountryId == request.CountryId.Value);
        }

        var result = await query
            .Select(m => new MunicipalityLookupResponse
            {
                ID = m.ID,
                Name = m.Name,
                CityID = m.CityId,
                CityName = m.City!.Name,
                RegionID = m.City.RegionId,
                RegionName = m.City.Region!.Name,
                CountryID = m.City.Region.CountryId,
                CountryName = m.City.Region.Country!.Name
            })
            .ToArrayAsync(cancellationToken);

        return result;
    }

    // DTO za zahtjev
    public class MunicipalityLookupRequest
    {
        public int? CountryId { get; set; } // Opcionalni filter za CountryId
        public int? RegionId { get; set; } // Opcionalni filter za RegionId
        public int? CityId { get; set; } // Opcionalni filter za CityId
    }

    // DTO za odgovor
    public class MunicipalityLookupResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required int CityID { get; set; }
        public required string CityName { get; set; }
        public required int RegionID { get; set; }
        public required string RegionName { get; set; }
        public required int CountryID { get; set; }
        public required string CountryName { get; set; }
    }
}
