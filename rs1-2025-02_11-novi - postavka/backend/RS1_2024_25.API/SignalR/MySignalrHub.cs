using Microsoft.AspNetCore.SignalR;
using RS1_2024_25.API.Services;

namespace RS1_2024_25.API.SignalRHubs;

public class MySignalrHub(IMyAuthService myAuthService) : Hub
{
    private string? GetMyAuthToken()
    {
        return Context.GetHttpContext()?.Request.Query["my-auth-token"];
    }

    public override async Task OnConnectedAsync()
    {
        // Dohvati token iz query stringa
        var tokenString = GetMyAuthToken();

        if (string.IsNullOrEmpty(tokenString))
        {
            // Prekid konekcije ako token nije poslan
            throw new HubException("Unauthorized: Token is missing.");
        }

        // Validacija tokena i dohvatanje korisničkih informacija
        MyAuthInfo authInfo = myAuthService.GetAuthInfoFromTokenString(tokenString);

        if (!authInfo.IsLoggedIn)
        {
            // Prekid konekcije ako token nije validan
            throw new HubException("Unauthorized: Invalid token.");
        }

        // Dodavanje korisnika u grupu na osnovu njegovog korisničkog emaila/username
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{authInfo.Email}");

        Console.WriteLine($"User {authInfo.Email} connected with ConnectionId {Context.ConnectionId}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Dohvati token iz query stringa
        var tokenString = GetMyAuthToken();

        if (!string.IsNullOrEmpty(tokenString))
        {
            var authInfo = myAuthService.GetAuthInfoFromTokenString(tokenString);

            if (authInfo.IsLoggedIn)
            {
                // Uklanjanje korisnika iz grupe
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{authInfo.Email}");
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Metoda za slanje poruke korisniku
    public async Task MyServerHubMethod1(string toUser, string message)
    {
        var tokenString = GetMyAuthToken();

        var authInfo = myAuthService.GetAuthInfoFromTokenString(tokenString);

        if (!authInfo.IsLoggedIn)
            throw new HubException("Unauthorized.");

        // Slanje poruke useru toUser
        await Clients.Group($"user_{toUser}")
                     .SendAsync("myClientMethod1", message);
    }
}
