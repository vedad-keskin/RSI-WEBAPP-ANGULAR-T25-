using RS1_2024_25.API.Endpoints.AuthEndpoints;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Services;
using Xunit;
using Microsoft.AspNetCore.Mvc;

public class AuthLoginEndpointTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMyAuthService _authService;
    private readonly AuthLoginEndpoint _authLoginEndpoint;

    public AuthLoginEndpointTests()
    {
        _dbContext = TestApplicationDbContext.CreateAsync().GetAwaiter().GetResult();
        _authService = new MyAuthService(_dbContext, TestHttpContextAccessorHelper.CreateWithValidAuthToken());
        _authLoginEndpoint = new AuthLoginEndpoint(_dbContext, _authService);
    }

    [Fact]
    public async Task Should_Return_Token_When_Valid_Credentials()
    {
        // Arrange
        var request = new AuthLoginEndpoint.LoginRequest
        {
            Email = "admin",
            Password = "test"
        };

        // Act
        ActionResult<AuthLoginEndpoint.LoginResponse> result = await _authLoginEndpoint.HandleAsync(request);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
        AuthLoginEndpoint.LoginResponse response = Assert.IsType<AuthLoginEndpoint.LoginResponse>(okResult.Value);

        Assert.NotNull(response.Token);
        Assert.NotNull(response.MyAuthInfo);
        Assert.True(response.MyAuthInfo.IsLoggedIn);
        Assert.Equal("admin", response.MyAuthInfo.Email);
    }

    [Fact]
    public async Task Should_Return_Unauthorized_When_Invalid_Credentials()
    {
        // Arrange
        var request = new AuthLoginEndpoint.LoginRequest
        {
            Email = "admin",
            Password = "wrongpassword"
        };

        // Act
        var result = await _authLoginEndpoint.HandleAsync(request);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }
}
