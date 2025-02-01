using Microsoft.AspNetCore.Http;
using Moq;

public static class TestHttpContextAccessorHelper
{
    public static string ValidTokenValue { get; set; } = "moj-token-123";

    public static IHttpContextAccessor CreateWithValidAuthToken()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var context = new DefaultHttpContext();
        context.Request.Headers["my-auth-token"] = ValidTokenValue;

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

        return mockHttpContextAccessor.Object;
    }

    public static IHttpContextAccessor CreateWithInvalidAuthToken()
    {
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var context = new DefaultHttpContext();
        context.Request.Headers["my-auth-token"] = "non-valid-token-123";

        mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

        return mockHttpContextAccessor.Object;
    }
}
