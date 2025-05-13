using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models
{
    public class Semester
    {
        public int ID { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; } // FK na studenta
        public Student Student { get; set; } // Referenca na korisnički entitet

        [ForeignKey(nameof(AcademicYear))]
        public int AcademicYearId { get; set; } // FK na akademsku godinu
        public AcademicYear AcademicYear { get; set; }

        [ForeignKey(nameof(RecordedBy))]
        public int? RecordedById { get; set; } // FK na usera koji evidentira
        public MyAppUser? RecordedBy { get; set; }

        public int YearOfStudy { get; set; }
        public bool Renewal { get; set; }

        public DateTime Date { get; set; }

        public int Price { get; set; }
    }
}
