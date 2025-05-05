using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;

[MyAuthorization(isAdmin: true, isManager: false)]
[Route("students")]
public class StudentRestoreEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{
    [HttpPost("{id}/restore")]
    public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await db.Students.SingleOrDefaultAsync(x => x.ID == id, cancellationToken);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (!student.IsDeleted)
            throw new Exception("Student is not deleted");

        // Restore
        student.IsDeleted = false;
        await db.SaveChangesAsync(cancellationToken);
    }
}
