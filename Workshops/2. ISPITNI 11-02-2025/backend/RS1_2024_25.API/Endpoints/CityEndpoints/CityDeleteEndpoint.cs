namespace RS1_2024_25.API.Endpoints.CityEndpoints;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using System.Threading;
using System.Threading.Tasks;

[MyAuthorization(isAdmin: true, isManager : false)]
[Route("cities")]
public class CityDeleteEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{

    [HttpDelete("{id}")]
    public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var city = await db.Cities.SingleOrDefaultAsync(x => x.ID == id, cancellationToken);

        if (city == null)
            throw new KeyNotFoundException("City not found");

        int count = await db.Municipalities.CountAsync(x => x.CityId == id, cancellationToken);
        if (count > 0)
        {
            throw new Exception($"City id {id} has {count} Municipalities");
        }

        db.Cities.Remove(city);
        await db.SaveChangesAsync(cancellationToken);
    }
}

