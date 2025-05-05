using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using static RS1_2024_25.API.Endpoints.CityEndpoints.CityGetByIdEndpoint;

namespace RS1_2024_25.API.Endpoints.CityEndpoints;

[Route("cities")]
public class CityGetByIdEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithActionResult<CityGetByIdResponse>
{
    [HttpGet("{id}")]
    public override async Task<ActionResult<CityGetByIdResponse>> HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var city = await db.Cities
                            .Where(c => c.ID == id)
                            .Select(c => new CityGetByIdResponse
                            {
                                ID = c.ID,
                                Name = c.Name,
                                RegionId = c.RegionId,
                                CountryId = c.Region!.CountryId
                            })
                            .FirstOrDefaultAsync(x => x.ID == id, cancellationToken);

        if (city == null)
        {
            throw new ArgumentException("City not found");
        }


        return Ok(city);
    }

    public class CityGetByIdResponse
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public required int RegionId { get; set; }
        public required int CountryId { get; set; }
    }
}
