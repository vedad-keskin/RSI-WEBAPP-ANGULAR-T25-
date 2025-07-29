namespace RS1_2024_25.API.Endpoints.StudentEndpoints;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using System.Threading;
using System.Threading.Tasks;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.StudentDeleteEndpoint;

[MyAuthorization(isAdmin: true, isManager: false)]
[Route("students")]
public class StudentDeleteEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<StudentDeleteRequest>
    .WithoutResult
{
    [HttpDelete]
    public override async Task HandleAsync(StudentDeleteRequest request, CancellationToken cancellationToken = default)
    {
        var student = await db.Students.SingleOrDefaultAsync(x => x.ID == request.ID, cancellationToken);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (student.IsDeleted)
            throw new Exception("Student is already deleted");

        // Soft-delete
        student.IsDeleted = true;
        student.TimeDeleted = DateTime.Now;
        student.UserDeletedId = request.UserDeletedId;


        await db.SaveChangesAsync(cancellationToken);
    }

    public class StudentDeleteRequest
    {
        public int ID { get; set; }
        public int? UserDeletedId { get; set; } // FK na grad


    }

}
