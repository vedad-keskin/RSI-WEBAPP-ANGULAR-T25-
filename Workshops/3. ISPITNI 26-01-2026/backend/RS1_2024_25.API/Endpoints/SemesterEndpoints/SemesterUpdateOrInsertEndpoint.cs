using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.SemesterEndpoints.SemesterUpdateOrInsertEndpoint;

namespace RS1_2024_25.API.Endpoints.SemesterEndpoints;

[Route("students")]
[MyAuthorization(isAdmin: true, isManager: false)]
public class SemesterUpdateOrInsertEndpoint(ApplicationDbContext db, IMyAuthService authService) : MyEndpointBaseAsync
    .WithRequest<SemesterUpdateOrInsertRequest>
    .WithActionResult<int>
{
    [HttpPost("{studentId}/semesters")]
    public override async Task<ActionResult<int>> HandleAsync(
        [FromBody] SemesterUpdateOrInsertRequest request,
        CancellationToken cancellationToken = default)
    {
        // studentId comes only from route; body is ignored for this field
        var studentId = int.Parse( HttpContext.GetRouteValue("studentId").ToString());
        

        var authInfo = authService.GetAuthInfoFromRequest();



        // Check if it's an insert or update operation
        bool isInsert = (request.ID == null || request.ID == 0);
        Semester? semester;

        if (isInsert)
        {
            
            semester = new Semester();

            db.Add(semester);
        }
        else
        {
            // Update operation: retrieve the existing student
            semester = await db.Semesters
                .SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken);

            if (semester == null)
            {
                return NotFound("Semester not found");
            }
        }


        if (db.SemestersAll.ToList().Exists(x => x.StudentId == studentId && x.AcademicYearId == request.AcademicYearId))
        {
            throw new InvalidOperationException("Academic Year already exists");
        }

        // Set common properties for both insert and update
        semester.AcademicYearId = request.AcademicYearId;
        semester.StudyYear = request.StudyYear;
        semester.EnrollmentDate = request.EnrollmentDate;
        semester.TenantId = 1;
        semester.RecordedById = authInfo.UserId;
        semester.StudentId = studentId;

        // IsRenewal and TuitionFee Logic

        var countGodine = db.SemestersAll
            .Where(x => x.StudentId == studentId && x.StudyYear == request.StudyYear)
            .Count();

        if(countGodine == 0)
        {
            semester.TuitionFee = 1800;
            semester.IsRenewal = false;
        }
        else if (countGodine == 1)
        {

            semester.TuitionFee = 400;
            semester.IsRenewal = true;

        }
        else
        {
            semester.TuitionFee = 500;
            semester.IsRenewal = true;

        }



        // Save changes to the database
        await db.SaveChangesAsync(cancellationToken);

        return Ok(semester.ID); // Return the ID of the student
    }

    public class SemesterUpdateOrInsertRequest
    {
        public int? ID { get; set; } // Nullable to allow null for insert operations
        public int AcademicYearId { get; set; }
        public int StudyYear { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
