using System;
using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Endpoints.CityEndpoints;
using Xunit;

public class CityGetByIdEndpointTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CityGetByIdEndpoint _endpoint;

    public CityGetByIdEndpointTests()
    {
        // Kreiranje TestApplicationDbContext sa seedanim podacima
        _dbContext = TestApplicationDbContext.CreateAsync().GetAwaiter().GetResult();

        // Inicijalizacija endpoint-a
        _endpoint = new CityGetByIdEndpoint(_dbContext);
    }

    [Fact]
    public async Task Should_Return_City_When_Valid_Id()
    {
        // Arrange
        int validCityId = 1;  // Preuzet iz seed podataka

        // Act
        var result = await _endpoint.HandleAsync(validCityId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var city = Assert.IsType<CityGetByIdEndpoint.CityGetByIdResponse>(okResult.Value);

        Assert.Equal(validCityId, city.ID);
        Assert.False(string.IsNullOrEmpty(city.Name));
        Assert.True(city.RegionId > 0);
        Assert.True(city.CountryId > 0);
    }

    [Fact]
    public async Task Should_Return_NotFound_When_Invalid_Id()
    {
        // Arrange
        int invalidCityId = 999;  // ID koji ne postoji u seed podacima

        // Act
        var result = await _endpoint.HandleAsync(invalidCityId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);

        await Assert.ThrowsAsync<ArgumentException>(() => _endpoint.HandleAsync(invalidCityId));
    }

    [Fact]
    public async Task Should_Return_Correct_City_Data()
    {
        // Arrange
        int cityId = 1;  // Seed podaci
        string expectedCityName = "Mostar";  // Iz seedera

        // Act
        var result = await _endpoint.HandleAsync(cityId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var city = Assert.IsType<CityGetByIdEndpoint.CityGetByIdResponse>(okResult.Value);

        Assert.Equal(cityId, city.ID);
        Assert.Equal(expectedCityName, city.Name);
        Assert.Equal(1, city.RegionId);  // Prema seed podacima
        Assert.Equal(1, city.CountryId);  // Prema seed podacima
    }
}
