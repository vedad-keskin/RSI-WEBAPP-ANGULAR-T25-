using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.CityEndpoints.CityGetAll3Endpoint;

namespace RS1_2024_25.API.Endpoints.CityEndpoints;

//sa paging i sa filterom
[Route("cities")]
public class CityGetAll3Endpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<CityGetAll3Request>
    .WithResult<MyPagedList<CityGetAll3Response>>
{
    [HttpGet("filter")]
    public override async Task<MyPagedList<CityGetAll3Response>> HandleAsync([FromQuery] CityGetAll3Request request, CancellationToken cancellationToken = default)
    {
        // Kreiranje osnovnog query-a
        var query = db.Cities
            .AsQueryable();

        // Primjena filtera na osnovu naziva grada
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            query = query.Where(c => c.Name.ToLower().Contains(request.Q) ||
                c.Region!.Name.ToLower().Contains(request.Q) ||
                c.Region!.Country!.Name.ToLower().Contains(request.Q)
            );
        }

        // Projektovanje u rezultatni tip
        var projectedQuery = query.Select(c => new CityGetAll3Response
        {
            ID = c.ID,
            Name = c.Name,
            RegionName = c.Region!.Name,
            CountryName = c.Region!.Country!.Name
        });

        // Kreiranje paginiranog odgovora sa filterom
        var result = await MyPagedList<CityGetAll3Response>.CreateAsync(projectedQuery, request, cancellationToken);


        return result;
    }
    public class CityGetAll3Request : MyPagedRequest //naslijeđujemo
    {
        public string? Q { get; set; } = string.Empty;
    }

    public class CityGetAll3Response
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required string RegionName { get; set; }
        public required string CountryName { get; set; }
    }
}