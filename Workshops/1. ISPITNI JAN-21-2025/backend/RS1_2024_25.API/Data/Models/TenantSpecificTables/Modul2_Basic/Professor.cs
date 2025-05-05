namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;

using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Professor: TenantSpecificTable
{
    [ForeignKey(nameof(User))]
    public int UserId { get; set; } // FK na korisnika
    public MyAppUser User { get; set; } // Referenca na korisnički entitet

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } // Npr. "Dr.", "Mr.", "Prof."

    [Required]
    public string Department { get; set; } // Odsjek (npr. "Računarstvo", "Matematika")

    public DateTime HireDate { get; set; } // Datum zapošljavanja
    public DateTime? EndDate { get; set; } // Datum do kada je radio (null ako je još aktivan)

    [MaxLength(500)]
    public string? Biography { get; set; } // Kratka biografija (opciono)
}
