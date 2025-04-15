namespace RS1_2024_25.API.Endpoints.CityEndpoints;

using FluentValidation;
using RS1_2024_25.API.Data;

public class CityUpdateOrInsertValidator : AbstractValidator<CityUpdateOrInsertEndpoint.CityUpdateOrInsertRequest>
{
    public CityUpdateOrInsertValidator(ApplicationDbContext dbContext)
    {
        // Validacija naziva grada
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Naziv grada je obavezan.")
            .MaximumLength(100).WithMessage("Naziv grada ne smije biti duži od 100 karaktera.")
            .Matches("^[a-zA-Z0-9 šđčćžŠĐČĆŽ-]+$").WithMessage("Naziv grada može sadržavati samo slova, brojeve, razmake i crtice.");

        // Validacija CountryId
        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("ID države mora biti veći od 0.");

        // Validacija RegionId
        RuleFor(x => x.RegionId)
            .GreaterThan(0).WithMessage("ID regije mora biti veći od 0.");

        // Dodatna poslovna pravila za validaciju povezanih entiteta
        // Ako je ukljucena autovalidacija u "program.cs"
        // builder.Services.AddFluentValidationAutoValidation();
        // onda validacija ne može biti async.
        // Ovo je bug na online nastavi.
        RuleFor(x => x)
                 .Must(request =>
                 {
                     var region = dbContext.Regions.FirstOrDefault(r => r.ID == request.RegionId);
                     return region != null && region.CountryId == request.CountryId;
                 })
            .WithMessage("Regija nije validna za datu državu.");
    }
}
