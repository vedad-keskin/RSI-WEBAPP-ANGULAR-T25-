using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;

namespace RS1_2024_25.API.Services
{
    public interface IMyAuthService
    {
        Task<MyAuthenticationToken> GenerateSaveAuthToken(MyAppUser user, CancellationToken cancellationToken = default);
        Task<bool> RevokeAuthToken(string tokenValue, CancellationToken cancellationToken = default);
        MyAuthInfo GetAuthInfoFromTokenString(string? authToken);
        MyAuthInfo GetAuthInfoFromRequest();
        MyAuthInfo GetAuthInfoFromTokenModel(MyAuthenticationToken? myAuthToken);
    }

}
