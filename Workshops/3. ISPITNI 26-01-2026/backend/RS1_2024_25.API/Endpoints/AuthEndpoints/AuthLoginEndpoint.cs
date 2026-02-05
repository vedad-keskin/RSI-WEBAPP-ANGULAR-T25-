using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using System.Threading;
using System.Threading.Tasks;
using static RS1_2024_25.API.Endpoints.AuthEndpoints.AuthLoginEndpoint;

namespace RS1_2024_25.API.Endpoints.AuthEndpoints;

[Route("auth")]
public class AuthLoginEndpoint(ApplicationDbContext db, IMyAuthService authService) : MyEndpointBaseAsync
    .WithRequest<LoginRequest>
    .WithActionResult<LoginResponse>
{
    [HttpPost("login")]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        // Provjera da li korisnik postoji u bazi
        var loggedInUser = await db.MyAppUsersAll
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (loggedInUser == null || !loggedInUser.VerifyPassword(request.Password))
        {
            // Sačuvaj promjene samo ako je korisnik postojao i ako su povećani neuspješni pokušaji
            if (loggedInUser != null)
            {
                db.CurrentTenantId = loggedInUser.TenantId;  //rucno postavljanje CurrentTenantId u dbContext zbog db inserta AuthToken
                await db.SaveChangesAsync(cancellationToken);
            }
            return Unauthorized(new { Message = "Incorrect username or password" });
        }

        db.CurrentTenantId = loggedInUser.TenantId; //rucno postavljanje CurrentTenantId u dbContext zbog db inserta AuthToken
        // Generisanje novog autentifikacionog tokena
        MyAuthenticationToken newAuthToken = await authService.GenerateSaveAuthToken(loggedInUser, cancellationToken);
        MyAuthInfo authInfo = authService.GetAuthInfoFromTokenModel(newAuthToken);

        return Ok(new LoginResponse
        {
            Token = newAuthToken.Value,
            MyAuthInfo = authInfo
        });

    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponse
    {
        public required MyAuthInfo? MyAuthInfo { get; set; }
        public string Token { get; internal set; }
    }

    /*
    chatgpt:

    Razlika između ove custom implementacije autentifikacije i korištenja JWT (JSON Web Token) autentifikacije 
    leži u nekoliko ključnih aspekata:

    1. Struktura i Sadržaj Tokena
    Custom implementacija: U ovom slučaju, token (randomToken) je generisan kao jednostavan slučajni niz karaktera, 
    obično bez dodatne informacije o korisniku. Token služi samo kao referenca za pretragu u bazi podataka. Svi podaci o korisniku i njegovim privilegijama se povlače sa servera na osnovu ovog tokena.

    JWT token: JWT je samostalni token koji sadrži šifrovane ili kodirane informacije o korisniku (npr. korisničko ime, 
    ID, uloge) unutar samog tokena, u dijelovima koji se nazivaju claims (zahtjevi). JWT je u formatu header.payload.
    signature, gdje payload dio sadrži kodirane podatke, dok signature služi za potvrdu autentičnosti i integriteta tokena.

    2. Provjera Autentičnosti i Sigurnost
    Custom implementacija: Prilikom svakog zahtjeva, aplikacija koristi token za traženje zapisa u bazi podataka 
    (u tabeli MyAuthenticationTokens). Ovo znači da aplikacija mora vršiti upit prema bazi pri svakom zahtjevu kako 
    bi provjerila autentičnost korisnika. Iako ovo može pružiti fleksibilnost, može opteretiti bazu podataka kod 
    aplikacija s velikim brojem korisnika.

    JWT: JWT se, s druge strane, može verifikovati bez potrebe za upitom prema bazi. Server jednostavno dešifruje 
    i provjerava potpis tokena. Ako je potpis validan, JWT se smatra autentičnim. To znači da nema potrebe za 
    dodatnim upitima prema bazi za osnovnu provjeru autentičnosti, što smanjuje opterećenje baze i povećava performanse.

    3. Trajanje i Istek Tokena
    Custom implementacija: Obično zahtijeva ručno upravljanje istekom tokena u bazi. To znači da svaki token može 
    imati polje RecordedAt ili ExpiresAt koje server mora povremeno provjeravati i ažurirati. Može biti komplikovano 
    upravljati isticanjem tokena i osigurati da stari tokeni budu obrisani iz baze.

    JWT: JWT sam po sebi može sadržavati informaciju o isteku (exp claim), koja omogućava automatsko upravljanje 
    trajanjem tokena bez potrebe za pohranjivanjem tokena na serveru. Ako je token istekao, server jednostavno 
    odbija zahtjev bez dodatne provjere u bazi.

    4. Sigurnost i Mogućnost Provale
    Custom implementacija: Token je samo referenca na zapis u bazi, što znači da ako bi neko presreo token, mogao 
    bi potencijalno pristupiti korisničkom nalogu dok je token validan. Također, zbog jednostavne prirode tokena 
    (npr. slučajni niz), ovaj pristup može biti ranjiviji na session hijacking (krađu sesije) u usporedbi sa JWT-om.

    JWT: JWT koristi kriptografski potpis koji potvrđuje integritet i autentičnost tokena. JWT tokeni se obično 
    potpisuju koristeći HMAC algoritam ili asimetričnu enkripciju (npr. RSA), što ih čini sigurnijim. Ipak, jednom 
    kada je JWT generisan, ne može se opozvati bez implementacije dodatnih mehanizama (što može biti kompleksno), 
    dok custom implementacija može jednostavno obrisati token iz baze da ga učini nevažećim.

    5. Revokacija Tokena (Povlačenje pristupa)
    Custom implementacija: Jednostavnija za implementaciju kada se radi o revokaciji. Token se može jednostavno 
    obrisati iz baze, i odmah postaje nevažeći. Ovo omogućava serveru da lako opozove pristup korisnicima koji su 
    blokirani ili koji više ne trebaju pristup.

    JWT: JWT token, s druge strane, nema ugrađen mehanizam za revokaciju jer je token samostalan i ne zavisi od 
    servera nakon generisanja. Za implementaciju revokacije potrebna je dodatna infrastruktura, kao što je lista 
    opozvanih tokena (revocation list) koja se provjerava pri svakom zahtjevu, ili implementacija kratkotrajnih 
    tokena u kombinaciji sa refresh tokenima.

    6. Performanse
    Custom implementacija: Zbog stalnog pristupa bazi podataka, može biti manje efikasna u aplikacijama sa velikim 
    brojem korisnika. Svaki zahtjev zahtijeva upit prema bazi za pronalaženje tokena i korisničkih podataka.

    JWT: Obično je efikasniji u performansama, jer ne zahtijeva upit prema bazi podataka za svaku provjeru. 
    Token se može verificirati bez dodatnog opterećenja baze, što ga čini pogodnim za aplikacije sa velikim 
    brojem korisnika ili distribuisanim sistemima.

    Zaključak
    Custom implementacija:

    Jednostavna za revokaciju i upravljanje istekom.
    Opterećuje bazu podataka jer zahtijeva upit za svaki zahtjev.
    Fleksibilna, ali može biti manje sigurna zbog jednostavne strukture tokena.
    JWT:

    Efikasniji u pogledu performansi jer ne zahtijeva bazu za osnovnu provjeru.
    Samostalan i šifrovan token koji sadrži sve potrebne podatke o korisniku.
    Kompleksniji za implementaciju revokacije, ali bolji za distribuisane sisteme i aplikacije sa visokim performansama.
    Ako vaša aplikacija zahtijeva česte revokacije i ima manji broj korisnika, custom implementacija može biti pogodnija. 
    Međutim, za veće, skalabilne sisteme sa većim brojem korisnika, JWT je često bolji izbor.


    Za studentsko učenje, custom implementacija autentifikacije je bolja jer je jednostavnija i razumljivija za početnike:

    Pojednostavljeni Koncepti: Custom implementacija koristi osnovne koncepte (generisanje tokena, pohrana u bazu, validacija),
    što je lakše za shvatiti.

    Praktičan Rad sa Bazom: Studenti vježbaju rad s bazom podataka, uče o relacijskim tabelama i optimizaciji upita.

    Laka Revokacija: Brisanje tokena iz baze jednostavno opoziva pristup, dok JWT zahtijeva složenije tehnike za revokaciju.

    Priprema za JWT: Custom implementacija pomaže studentima da shvate osnove autentifikacije, što olakšava prelazak na JWT.

    Preporuka: Počnite s custom implementacijom za osnovno razumijevanje, a zatim uvesti JWT kao naprednu temu.
     */

}
