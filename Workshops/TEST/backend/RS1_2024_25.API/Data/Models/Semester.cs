using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Semester
    {

//• datum upisa: DateTime
//• godina studija: int
//• akademska godina: FK na tabelu AkademskaGodina(prikazati unutar combobox-a)
//• cijena školarine: float disabled* (automatski se setuje na 400 ako student upisuje istu godinu
//studija, inače na 1800)
//• obnova: bool disabled* (automatski se setuje na true ako student upisuje istu godinu studija,
//inače na false)

        [Key]
        public int ID { get; set; }

        // kopirano iz Municipality
        public int AcademicYearId { get; set; } 

        [ForeignKey(nameof(AcademicYearId))]
        public AcademicYear? AcademicYear { get; set; }


        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }


        public int RecordedById { get; set; }

        [ForeignKey(nameof(RecordedById))]
        public MyAppUser? RecordedBy { get; set; }

        public DateTime Date { get; set; }

        public int YearOfStudy { get; set; }

        public float Price { get; set; }

        public bool Renewal { get; set; }

    }
}
