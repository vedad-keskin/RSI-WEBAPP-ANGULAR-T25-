using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Helper.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic
{
    public class Semester : TenantSpecificTable
    {
        //• StudentId(FK)
        //• AcademicYearId(FK na AcademicYear)
        //• EvidentiraoKorisnikId(FK na korisnika koji evidentira)

        public int AcademicYearId { get; set; }
        [ForeignKey(nameof(AcademicYearId))]
        public AcademicYear? AcademicYear { get; set; }


        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }

        public int RecordedById { get; set; }
        [ForeignKey(nameof(RecordedById))]
        public MyAppUser? RecordedBy { get; set; }

        //• StudyYear(int)
        //• EnrollmentDate(DateTime)
        //• TuitionFee(float/decimal)
        //• IsRenewal(bool)
        //• IsDeleted(bool) – zbog soft delete/restore na semestrima

        public int StudyYear { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public float TuitionFee { get; set; }
        public bool IsRenewal { get; set; }
        public bool IsDeleted { get; set; }
    }
}
