namespace RS1_2024_25.API.Endpoints.DataSeed
{
    using Microsoft.AspNetCore.Mvc;
    using RS1_2024_25.API.Data;
    using RS1_2024_25.API.Helper.Api;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    namespace FIT_Api_Example.Endpoints
    {
        [Route("data-seed")]
        public class DataSeedCountEndpoint(ApplicationDbContext db)
            : MyEndpointBaseAsync
            .WithoutRequest
            .WithResult<Dictionary<string, int>>
        {
            [HttpGet]
            public override async Task<Dictionary<string, int>> HandleAsync(CancellationToken cancellationToken = default)
            {
                Dictionary<string, int> dataCounts = new ()
                {
                    { "Country", db.Countries.Count() },
                    { "City", db.Cities.Count() },
                    { "MyAppUser", db.MyAppUsersAll.Count() }
                };

                return dataCounts;
            }
        }
    }

}
