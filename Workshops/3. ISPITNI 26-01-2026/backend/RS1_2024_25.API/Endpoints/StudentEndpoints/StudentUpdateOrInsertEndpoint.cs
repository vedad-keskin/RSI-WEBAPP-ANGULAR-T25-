using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Enums;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.StudentUpdateOrInsertEndpoint;

namespace RS1_2024_25.API.Endpoints.StudentEndpoints;

[Route("students")]
[MyAuthorization(isAdmin: true, isManager: false)]
public class StudentUpdateOrInsertEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<StudentUpdateOrInsertRequest>
    .WithActionResult<int>
{
    [HttpPost]  // Using POST to support both create and update
    public override async Task<ActionResult<int>> HandleAsync(
        [FromBody] StudentUpdateOrInsertRequest request,
        CancellationToken cancellationToken = default)
    {
  
        // Check if it's an insert or update operation
        bool isInsert = (request.ID == null || request.ID == 0);
        Student? student;

        if (isInsert)
        {
            // Insert operation: create a new student entity
            var newUser = new Data.Models.TenantSpecificTables.Modul1_Auth.MyAppUser();
            newUser.SetPassword("test");

            student = new Student
            {
                User = newUser,
            };
            db.Add(student);
        }
        else
        {
            // Update operation: retrieve the existing student
            student = await db.Students
                .Include(x=>x.User)
                .SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken);

            if (student == null)
            {
                return NotFound("Student not found");
            }
        }

        // Set common properties for both insert and update
        student.User.FirstName = request.FirstName;
        student.User.LastName= request.LastName;
        student.StudentNumber = request.StudentNumber;
        student.ParentName = request.ParentName;
        student.BirthDate = request.BirthDate;
        student.Gender = request.Gender;
        student.CitizenshipId = request.CitizenshipId;
        student.BirthPlace = request.BirthPlace;
        student.BirthMunicipalityId = request.BirthMunicipalityId;
        student.PermanentAddressStreet = request.PermanentAddressStreet;
        student.PermanentMunicipalityId = request.PermanentMunicipalityId;
        student.ContactMobilePhone = request.ContactMobilePhone;
        student.ContactPrivateEmail = request.ContactPrivateEmail;

        // Save changes to the database
        await db.SaveChangesAsync(cancellationToken);

        return Ok(student.ID); // Return the ID of the student
    }

    public class StudentUpdateOrInsertRequest
    {
        public int? ID { get; set; } // Nullable to allow null for insert operations
        public string? ParentName { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string StudentNumber { get; set; }
        public DateOnly? BirthDate { get; set; }
        public Gender Gender { get; set; }
        public int? CitizenshipId { get; set; }
        public string? BirthPlace { get; set; }
        public int? BirthMunicipalityId { get; set; }
        public string? PermanentAddressStreet { get; set; }
        public int? PermanentMunicipalityId { get; set; }        
        public string? ContactMobilePhone { get; set; }
        public string? ContactPrivateEmail { get; set; }
    }
}
