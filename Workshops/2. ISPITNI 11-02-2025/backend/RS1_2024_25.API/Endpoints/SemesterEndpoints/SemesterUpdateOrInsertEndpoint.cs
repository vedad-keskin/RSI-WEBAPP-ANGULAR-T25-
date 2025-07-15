using Microsoft.AspNetCore.Mvc;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.SemesterEndpoints.SemesterUpdateOrInsertEndpoint;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models;

namespace RS1_2024_25.API.Endpoints.SemesterEndpoints;


[Route("semesters")]
//[MyAuthorization(isAdmin: true, isManager: false)]
public class SemesterUpdateOrInsertEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<SemesterUpdateOrInsertRequest>
    .WithActionResult<int>
{
    [HttpPost]  // Using POST to support both create and update
    public override async Task<ActionResult<int>> HandleAsync(
        [FromBody] SemesterUpdateOrInsertRequest request,
        CancellationToken cancellationToken = default)
    {

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
            // Update operation: retrieve the existing semester
            semester = await db.Semesters
                .SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken);

            if (semester == null)
            {
                return NotFound("Semester not found");
            }
        }

        // Set common properties for both insert and update
        semester.AcademicYearId = request.AcademicYearId;
        semester.StudentId = request.StudentId;
        semester.RecordedById = request.RecordedById;
        semester.Price = request.Price;
        semester.DateOfEnrollment = request.DateOfEnrollment;
        semester.YearOfStudy = request.YearOfStudy;
        semester.Renewal = request.Renewal;

        // Save changes to the database
        await db.SaveChangesAsync(cancellationToken);

        return Ok(semester.ID); // Return the ID of the student
    }

    public class SemesterUpdateOrInsertRequest
    {
        public int? ID { get; set; } // Nullable to allow null for insert operations
        public int AcademicYearId { get; set; } // FK na AcademicYear
        public int StudentId { get; set; } // FK na Student
        public int RecordedById { get; set; } // FK na RecordedBy
        public DateTime DateOfEnrollment { get; set; }
        public int YearOfStudy { get; set; }
        public float Price { get; set; }
        public bool Renewal { get; set; }


    }
}

