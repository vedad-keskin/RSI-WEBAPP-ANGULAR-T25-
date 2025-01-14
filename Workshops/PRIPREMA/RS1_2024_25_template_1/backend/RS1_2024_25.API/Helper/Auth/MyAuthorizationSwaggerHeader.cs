using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RS1_2024_25.API.Helper.Auth;

public class MyAuthorizationSwaggerHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "my-auth-token",
            In = ParameterLocation.Header,
            Description = "upisati token preuzet iz autentikacijacontrollera"
        });
    }
}
