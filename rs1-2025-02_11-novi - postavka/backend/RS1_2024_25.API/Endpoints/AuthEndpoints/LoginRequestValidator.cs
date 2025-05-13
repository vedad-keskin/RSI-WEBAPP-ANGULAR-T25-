namespace RS1_2024_25.API.Endpoints.AuthEndpoints;

using FluentValidation;
using static RS1_2024_25.API.Endpoints.AuthEndpoints.AuthLoginEndpoint;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        // Validacija Email polja
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email je obavezan.");
            //.EmailAddress().WithMessage("Email adresa nije validna.");

        // Validacija Password polja
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Lozinka je obavezna.");
            //.MinimumLength(8).WithMessage("Lozinka mora imati najmanje 8 karaktera.")
            //.Matches(@"[A-Z]").WithMessage("Lozinka mora sadržavati barem jedno veliko slovo.")
            //.Matches(@"[a-z]").WithMessage("Lozinka mora sadržavati barem jedno malo slovo.")
            //.Matches(@"[0-9]").WithMessage("Lozinka mora sadržavati barem jedan broj.")
            //.Matches(@"[\!\@\#\$\%\^\&\*\(\)\_\+\-=\[\]\{\};:'"",<>\.\?\\\/]").WithMessage("Lozinka mora sadržavati barem jedan specijalni karakter.");
    }
}
