namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;

using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Semester : TenantSpecificTable
{


    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; } // FK na korisnika
    public Student Student { get; set; } // Referenca na korisnički entitet

    [ForeignKey(nameof(AcademicYear))]
    public int AcademicYearId { get; set; } // FK na korisnika
    public AcademicYear AcademicYear { get; set; } // Referenca na korisnički entitet

    [ForeignKey(nameof(RecordedBy))]
    public int? RecordedById { get; set; } // FK na korisnika
    public MyAppUser? RecordedBy { get; set; } // Referenca na korisnički entitet

    public int YearOfStudy { get; set; }
    public bool Renewal { get; set; }

    public DateTime Date { get; set; }

    public int Price { get; set; }


}
