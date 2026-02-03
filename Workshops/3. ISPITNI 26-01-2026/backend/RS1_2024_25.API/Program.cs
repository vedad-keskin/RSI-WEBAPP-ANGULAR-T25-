using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Auth;
using RS1_2024_25.API.Services;
using RS1_2024_25.API.SignalRHubs;
using FluentValidation;
using FluentValidation.AspNetCore;
using RS1_2024_25.API.Endpoints.AuthEndpoints;

var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", false)
.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("db1")));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.OperationFilter<MyAuthorizationSwaggerHeader>());
builder.Services.AddHttpContextAccessor();

//dodajte vaše servise
builder.Services.AddTransient<IMyAuthService, MyAuthService>();
builder.Services.AddSignalR();

builder.Services.AddFluentValidationAutoValidation();

//pretrazuje sve validatore iz DLL fajla (tj. projekta) koji sadrži AuthGetEndpoint.css
builder.Services.AddValidatorsFromAssemblyContaining<AuthGetEndpoint>();//moze se navesti bilo koja klasa iz ovog projekta

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(
    options => options
        .SetIsOriginAllowed(x => _ = true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
); //This needs to set everything allowed


app.UseAuthorization();

app.MapControllers();
app.MapHub<MySignalrHub>("/mysginalr-hub-path");

app.Run();
