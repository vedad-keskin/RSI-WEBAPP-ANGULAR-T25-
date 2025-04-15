using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper;

namespace RS1_2024_25.API.Services
{
    public class MyAuthServiceHelper
        //(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor, MyTokenGenerator myTokenGenerator)
    {

        // Generisanje novog tokena za korisnika
        public static async Task<MyAuthenticationToken> GenerateSaveAuthToken(string? IpAddress, ApplicationDbContext applicationDbContext, MyAppUser user, CancellationToken cancellationToken = default)
        {
            string randomToken = MyTokenGenerator.Generate(10);

            var authToken = new MyAuthenticationToken
            {
                IpAddress = IpAddress ?? string.Empty,
                Value = randomToken,
                MyAppUser = user,
                RecordedAt = DateTime.Now,
                TenantId = user.TenantId,
            };

            applicationDbContext.Add(authToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return authToken;
        }

        // Uklanjanje tokena iz baze podataka
        public static async Task<bool> RevokeAuthToken(ApplicationDbContext applicationDbContext, string tokenValue, CancellationToken cancellationToken = default)
        {
            var authToken = await applicationDbContext.MyAuthenticationTokens
                .FirstOrDefaultAsync(t => t.Value == tokenValue, cancellationToken);

            if (authToken == null)
                return false;

            applicationDbContext.Remove(authToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        // Dohvatanje informacija o autentifikaciji korisnika
        public static MyAuthInfo GetAuthInfoFromTokenString(ApplicationDbContext applicationDbContext, string? authToken)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                return GetAuthInfoFromTokenModel(null);
            }

            MyAuthenticationToken? myAuthToken = applicationDbContext.MyAuthenticationTokens
                .IgnoreQueryFilters()
                .Include(x => x.MyAppUser!.Tenant)
                .SingleOrDefault(x => x.Value == authToken);

            return GetAuthInfoFromTokenModel(myAuthToken);
        }


        // Dohvatanje informacija o autentifikaciji korisnika
        public static MyAuthInfo GetAuthInfoFromRequest(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            string? authToken = httpContextAccessor.HttpContext?.Request.Headers["my-auth-token"];
            return GetAuthInfoFromTokenString(applicationDbContext, authToken);
        }

        public static MyAuthInfo GetAuthInfoFromTokenModel(MyAuthenticationToken? myAuthToken)
        {
            if (myAuthToken == null)
            {
                return new MyAuthInfo
                {
                    IsAdmin = false,
                    IsDean = false,
                    IsLoggedIn = false,
                };
            }

            return new MyAuthInfo
            {
                UserId = myAuthToken.MyAppUserId,
                Email = myAuthToken.MyAppUser!.Email,
                FirstName = myAuthToken.MyAppUser.FirstName,
                LastName = myAuthToken.MyAppUser.LastName,
                IsAdmin = myAuthToken.MyAppUser.IsAdmin,
                IsDean = myAuthToken.MyAppUser.IsDean,
                IsLoggedIn = true,
                Tenant = myAuthToken.MyAppUser.Tenant,
                TenantId = myAuthToken.MyAppUser.TenantId,
            };
        }
    }

    // DTO to hold authentication information
    public class MyAuthInfo
    {
        public int TenantId { get; set; }
        [JsonIgnore]
        public Tenant? Tenant { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDean { get; set; }
        public bool IsStudent { get; set; }
        public bool IsProfessor { get; set; }
        public bool IsLoggedIn { get; set; }
        public string SlikaPath {  get; set; }
    }
}
