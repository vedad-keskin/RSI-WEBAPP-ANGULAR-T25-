using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Semester : BaseEntity
    {

        //• akademska godina: FK na tabelu AkademskaGodina(prikazati unutar combobox-a)


        // FK na AcademyYear
        public int AcademicYearId { get; set; } // FK na AcademicYear
        [ForeignKey(nameof(AcademicYearId))]
        public AcademicYear? AcademicYear { get; set; } // Navigaciona veza na AcademicYear

        // FK na Student
        public int StudentId { get; set; } // FK na Student
        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; } // Navigaciona veza na Student

        // FK na RecordedBy
        public int RecordedById { get; set; } // FK na RecordedBy
        [ForeignKey(nameof(RecordedById))]
        public MyAppUser? RecordedBy { get; set; } // Navigaciona veza na RecordedBy

        //• datum upisa: DateTime
        //• godina studija: int
        //• cijena školarine: float disabled* (automatski se setuje na 400 ako student upisuje istu godinu
        //studija, inače na 1800)
        //• obnova: bool disabled* (automatski se setuje na true ako student upisuje istu godinu studija,
        //inače na false)

        public DateTime DateOfEnrollment { get; set; }
        public int YearOfStudy { get; set; }
        public float Price { get; set; }
        public bool Renewal { get; set; }

    }
}
