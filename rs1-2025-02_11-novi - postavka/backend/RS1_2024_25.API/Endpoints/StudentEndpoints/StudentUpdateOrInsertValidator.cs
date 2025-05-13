using FluentValidation;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Endpoints.StudentEndpoints;
using RS1_2024_25.API.Data.Enums;

public class StudentUpdateOrInsertValidator : AbstractValidator<StudentUpdateOrInsertEndpoint.StudentUpdateOrInsertRequest>
{
    public StudentUpdateOrInsertValidator(ApplicationDbContext dbContext)
    {

        // Validacija StudentNumber
        RuleFor(x => x.StudentNumber)
            .NotEmpty().WithMessage("Broj indeksa je obavezan.")
            .Matches("^[A-Z]{2,3}[0-9]{6}$").WithMessage("Broj indeksa mora imati format: 2 ili 3 velika slova + 6 cifara.");


        // Validacija Gender
        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Pol mora biti validan (Male, Female, Other).");

        // Validacija CitizenshipId
        RuleFor(x => x.CitizenshipId)
            .GreaterThan(0).When(x => x.CitizenshipId.HasValue)
            .WithMessage("ID državljanstva mora biti veći od 0.");

        // Validacija BirthMunicipalityId
        RuleFor(x => x.BirthMunicipalityId)
            .GreaterThan(0).When(x => x.BirthMunicipalityId.HasValue)
            .WithMessage("ID opštine rođenja mora biti veći od 0.");

        // Validacija PermanentMunicipalityId
        RuleFor(x => x.PermanentMunicipalityId)
            .GreaterThan(0).When(x => x.PermanentMunicipalityId.HasValue)
            .WithMessage("ID opštine prebivališta mora biti veći od 0.");

        // Validacija datuma rođenja
        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .When(x => x.BirthDate.HasValue)
            .WithMessage("Datum rođenja ne može biti u budućnosti.");

        // Validacija ParentName
        RuleFor(x => x.ParentName)
            .MaximumLength(100).WithMessage("Ime roditelja ne smije biti duže od 100 karaktera.");

        // Validacija BirthPlace
        RuleFor(x => x.BirthPlace)
            .MaximumLength(100).WithMessage("Mjesto rođenja ne smije biti duže od 100 karaktera.");

        // Validacija PermanentAddressStreet
        RuleFor(x => x.PermanentAddressStreet)
            .MaximumLength(200).WithMessage("Adresa prebivališta ne smije biti duža od 200 karaktera.");

        // Validacija ContactMobilePhone
        //RuleFor(x => x.ContactMobilePhone)
        //    .Matches(@"^\+?[0-9]{7,15}$")
        //    .When(x => !string.IsNullOrEmpty(x.ContactMobilePhone))
        //    .WithMessage("Mobilni telefon mora biti u ispravnom formatu (dozvoljeni samo brojevi i opcioni '+' na početku).");

        // Validacija ContactPrivateEmail
        RuleFor(x => x.ContactPrivateEmail)
            .EmailAddress().WithMessage("Email mora biti validan.")
            .When(x => !string.IsNullOrEmpty(x.ContactPrivateEmail));

        // Dodatna poslovna pravila
        RuleFor(x => x)
            .Must(request =>
            {
                if (request.BirthMunicipalityId.HasValue)
                {
                    return dbContext.Municipalities.Any(m => m.ID == request.BirthMunicipalityId.Value);
                }
                return true;
            })
            .WithMessage("Opština rođenja nije validna.");

        RuleFor(x => x)
            .Must(request =>
            {
                if (request.PermanentMunicipalityId.HasValue)
                {
                    return dbContext.Municipalities.Any(m => m.ID == request.PermanentMunicipalityId.Value);
                }
                return true;
            })
            .WithMessage("Opština prebivališta nije validna.");
    }
}
