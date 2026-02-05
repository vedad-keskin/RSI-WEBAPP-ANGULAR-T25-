using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.CityEndpoints.CityUpdateOrInsertEndpoint;
using static RS1_2024_25.API.Endpoints.CityEndpoints.SemesterUpdateOrInsertEndpoint;

namespace RS1_2024_25.API.Endpoints.CityEndpoints;

[Route("students")]
[MyAuthorization(isAdmin: true, isManager: false)]
public class SemesterUpdateOrInsertEndpoint(ApplicationDbContext db, IMyAuthService authService) : MyEndpointBaseAsync
    .WithRequest<SemesterUpdateOrInsertRequest>
    .WithActionResult<int>
{
    [HttpPost("{studentId}/semesters")]  // Using POST to support both create and update
    public override async Task<ActionResult<int>> HandleAsync([FromBody] SemesterUpdateOrInsertRequest request, CancellationToken cancellationToken = default)
    {

        var studentId = int.Parse(HttpContext.GetRouteValue("studentId").ToString());



        var authInfo = authService.GetAuthInfoFromRequest();



        // Check if we're performing an insert or update based on the ID value
        bool isInsert = (request.ID == null || request.ID == 0);
        Semester? semester;

        if (isInsert)
        {
            // Insert operation: create a new city entity
            semester = new Semester();
            db.Add(semester); // Add the new city to the context
        }
        else
        {
            // Update operation: retrieve the existing city
            semester = await db.Semesters.SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken);

            if (semester == null)
            {
                return NotFound("Semester not found");
            }
        }

        if (db.SemestersAll.ToList()
            .Exists(x => x.StudentId == studentId && 
            x.AcademicYearId == request.AcademicYearId))
        {

            throw new InvalidOperationException("Student has already enrolled in that academic year..."); 

        }


        // Set common properties for both insert and update operations
        semester.AcademicYearId = request.AcademicYearId;
        semester.StudyYear = request.StudyYear;
        semester.EnrollmentDate = request.EnrollmentDate;
        semester.StudentId = studentId;
        //semester.TenantId = 1;
        semester.TenantId = authInfo.TenantId;
        semester.RecordedById = authInfo.UserId;


        var countGodine = db.SemestersAll
            .Where(x => x.StudentId == studentId && x.StudyYear == request.StudyYear)
            .ToList().Count();

        if(countGodine == 0)
        {

            semester.TuitionFee = 1800;
            semester.IsRenewal = false;

        }else if(countGodine == 1)
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

        return Ok(semester.ID);
    }

    public class SemesterUpdateOrInsertRequest
    {
        public int? ID { get; set; } // Nullable to allow null for insert operations
        public int AcademicYearId { get; set; }
        public int StudyYear { get; set; }
        public DateTime EnrollmentDate { get; set; }

    }
}
