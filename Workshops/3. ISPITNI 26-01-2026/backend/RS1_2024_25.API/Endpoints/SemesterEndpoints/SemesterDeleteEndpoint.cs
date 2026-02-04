namespace RS1_2024_25.API.Endpoints.StudentEndpoints;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using System.Threading;
using System.Threading.Tasks;

//[MyAuthorization(isAdmin: true, isManager: false)]
[Route("student-semesters")]
public class SemesterDeleteEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<int>
    .WithoutResult
{
    [HttpDelete("{id}")]
    public override async Task HandleAsync(int id, CancellationToken cancellationToken = default)
    {
        var semester = await db.Semesters.SingleOrDefaultAsync(x => x.ID == id, cancellationToken);

        if (semester == null)
            throw new KeyNotFoundException("Semester not found");

        if (semester.IsDeleted)
            throw new Exception("Semester is already deleted");

        // Soft-delete
        semester.IsDeleted = true;


        await db.SaveChangesAsync(cancellationToken);
    }
}
