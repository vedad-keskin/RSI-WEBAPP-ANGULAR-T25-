using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.CityEndpoints.CityUpdateOrInsertEndpoint;

namespace RS1_2024_25.API.Endpoints.CityEndpoints;

[Route("cities")]
[MyAuthorization(isAdmin: true, isManager: false)]
public class CityUpdateOrInsertEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<CityUpdateOrInsertRequest>
    .WithActionResult<int>
{
    [HttpPost]  // Using POST to support both create and update
    public override async Task<ActionResult<int>> HandleAsync([FromBody] CityUpdateOrInsertRequest request, CancellationToken cancellationToken = default)
    {

        // Check if we're performing an insert or update based on the ID value
        bool isInsert = (request.ID == null || request.ID == 0);
        City? city;

        if (isInsert)
        {
            // Insert operation: create a new city entity
            city = new City();
            db.Cities.Add(city); // Add the new city to the context
        }
        else
        {
            // Update operation: retrieve the existing city
            city = await db.Cities.SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken);

            if (city == null)
            {
                return NotFound("City not found");
            }
        }

        // Set common properties for both insert and update operations
        city.Name = request.Name;
        city.RegionId = request.RegionId;

        // Save changes to the database
        await db.SaveChangesAsync(cancellationToken);

        return Ok(city.ID);
    }

    public class CityUpdateOrInsertRequest
    {
        public int? ID { get; set; } // Nullable to allow null for insert operations
        public required string Name { get; set; }
        public required int CountryId { get; set; }
        public required int RegionId { get; set; }
    }
}
