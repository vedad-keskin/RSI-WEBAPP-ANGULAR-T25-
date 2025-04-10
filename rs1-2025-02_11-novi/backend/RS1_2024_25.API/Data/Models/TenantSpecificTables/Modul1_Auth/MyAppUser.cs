using Microsoft.AspNetCore.Identity;
using RS1_2024_25.API.Helper.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;

public class MyAppUser : TenantSpecificTable
{
    public string Email { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    //----------------
    public bool IsAdmin { get; set; }
    public bool IsDean { get; set; }

    /*
     
     Ako sistem nije zamišljen da podržava česte promjene rola i 
     ako se dodavanje novih rola svodi na manje promjene u kodu, 
    tada može biti dovoljno koristiti boolean polja kao što su IsAdmin, IsManager itd. 
    
    Ovaj pristup je jednostavan i efektivan u situacijama gdje su role stabilne i unaprijed definirane.

    Međutim, glavna prednost korištenja role entiteta dolazi do izražaja kada aplikacija potencijalno raste i 
    zahtjeva kompleksnije role i ovlaštenja. U scenarijima gdje se očekuje veći broj različitih rola ili kompleksniji 
    sistem ovlaštenja, dodavanje nove bool varijable može postati nepraktično i otežati održavanje.

    Dakle, za stabilne sisteme s manjim brojem fiksnih rola, boolean polja su sasvim razumno rješenje.
     */

    // Number of failed login attempts
    public int FailedLoginAttempts { get; set; } = 0;

    // Timestamp for when the account might be locked (optional)
    public DateTime? LockoutUntil { get; set; }

    // Helper method for password hashing
    public void SetPassword(string password)
    {
        var hasher = new PasswordHasher<MyAppUser>();
        PasswordHash = hasher.HashPassword(this, password);
    }

    // Helper method for password verification
    public bool VerifyPassword(string password)
    {
        var hasher = new PasswordHasher<MyAppUser>();
        var result = hasher.VerifyHashedPassword(this, PasswordHash, password);
        if (result == PasswordVerificationResult.Success)
        {
            // Reset failed login attempts on successful login
            FailedLoginAttempts = 0;
            LockoutUntil = null; // Reset lockout if it was set
            return true;
        }
        else
        {
            // Increment failed login attempts on unsuccessful login
            FailedLoginAttempts++;
            return false;
        }
    }

    // Check if account is locked
    public bool IsLocked()
    {
        return LockoutUntil.HasValue && LockoutUntil.Value > DateTime.UtcNow;
    }

    // Lock account for a specific duration
    public void LockAccount(int minutes)
    {
        LockoutUntil = DateTime.UtcNow.AddMinutes(minutes);
    }
}
