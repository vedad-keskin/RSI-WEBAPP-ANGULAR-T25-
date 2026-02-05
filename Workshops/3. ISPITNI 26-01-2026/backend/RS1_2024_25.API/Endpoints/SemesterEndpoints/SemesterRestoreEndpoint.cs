using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;

//[MyAuthorization(isAdmin: true, isManager: false)]
[Route("student-semesters")]
public class SemesterRestoreEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{
    [HttpPost("{id}/restore")]
    public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var semester = await db.Semesters.SingleOrDefaultAsync(x => x.ID == id, cancellationToken);

        if (semester == null)
            throw new KeyNotFoundException("Semester not found");

        if (!semester.IsDeleted)
            throw new Exception("Semester is not deleted");

        // Restore
        semester.IsDeleted = false;


        await db.SaveChangesAsync(cancellationToken);
    }
}
