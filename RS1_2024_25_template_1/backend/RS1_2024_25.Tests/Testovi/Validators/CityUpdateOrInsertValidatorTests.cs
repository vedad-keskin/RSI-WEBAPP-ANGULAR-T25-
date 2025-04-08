using FluentValidation.TestHelper;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Endpoints.CityEndpoints;

public class CityUpdateOrInsertValidatorTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CityUpdateOrInsertValidator _validator;

    public CityUpdateOrInsertValidatorTests()
    {
        // Kreiranje TestApplicationDbContext
        _dbContext = TestApplicationDbContext.CreateAsync().GetAwaiter().GetResult();

        // Kreiraj validator
        _validator = new CityUpdateOrInsertValidator(_dbContext);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        var regija = _dbContext.Regions.First();

        // Testira da li validator vraća grešku za prazan Name.
        var request = new CityUpdateOrInsertEndpoint.CityUpdateOrInsertRequest
        {
            Name = "",
            CountryId = regija.CountryId,  // Preuzeto iz seed podataka
            RegionId = regija.ID   // Preuzeto iz seed podataka
        };

        var result = await _validator.TestValidateAsync(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Request_Is_Valid()
    {
        var regija = _dbContext.Regions.First();

        // Testira validan zahtjev (bez grešaka).
        var request = new CityUpdateOrInsertEndpoint.CityUpdateOrInsertRequest
        {
            Name = "Test City",
            CountryId = regija.CountryId,  // Preuzeto iz seed podataka
            RegionId = regija.ID   // Preuzeto iz seed podataka
        };

        var result = await _validator.TestValidateAsync(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Should_Have_Error_When_Region_Is_Invalid()
    {
        var regija = _dbContext.Regions.First();
        // Testira da li validator vraća grešku za nepostojeći RegionId.
        var request = new CityUpdateOrInsertEndpoint.CityUpdateOrInsertRequest
        {
            Name = "Test City",
            CountryId = regija.CountryId,  // Preuzeto iz seed podataka
            RegionId = 999999999 // Region koji ne postoji
        };

        var result = await _validator.TestValidateAsync(request);
        result.ShouldHaveValidationErrorFor(x => x);
    }

    [Fact]
    public async Task Should_Have_Error_When_CountryId_Is_Invalid()
    {
        var regija = _dbContext.Regions.First();
        // Testira da li validator vraća grešku za nepostojeći CountryId.
        var request = new CityUpdateOrInsertEndpoint.CityUpdateOrInsertRequest
        {
            Name = "Test City",
            CountryId = 999999999, // Country koji ne postoji
            RegionId = regija.ID    // Preuzeto iz seed podataka
        };

        var result = await _validator.TestValidateAsync(request);
        result.ShouldHaveValidationErrorFor(x => x);
    }
}
