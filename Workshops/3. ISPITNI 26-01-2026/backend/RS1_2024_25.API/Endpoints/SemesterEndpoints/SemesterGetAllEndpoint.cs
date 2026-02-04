using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using static RS1_2024_25.API.Endpoints.SemesterEndpoints.SemesterGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.SemesterEndpoints;

[Route("students")]
//[MyAuthorization(isAdmin: true, isManager: false)]
public class SemesterGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<SemesterGetAllRequest>
    .WithResult<MyPagedList<SemesterGetAllResponse>>
{
    [HttpGet("{studentId}/semesters/filter")]
    public override async Task<MyPagedList<SemesterGetAllResponse>> HandleAsync(
        SemesterGetAllRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = db.Semesters
            .Include(x => x.Student.User)
            .Include(x => x.RecordedBy)
            .Include(x => x.AcademicYear)
            .Where(s => s.StudentId == request.StudentId)
            .AsQueryable();



        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            switch (request.Status.ToLower())
            {
                case "active":
                    query = query.Where(s => !s.IsDeleted);
                    break;

                case "deleted":
                    query = query.Where(s => s.IsDeleted);
                    break;

                case "all":
                    // ništa ne filtrira — vraća sve
                    break;
            }
        }

        // Primjena filtera po imenu, prezimenu, student broju ili državi
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            query = query.Where(s =>
                s.AcademicYear.Description.Contains(request.Q) 
            );
        }

        var projectedQuery = query.Select(s => new SemesterGetAllResponse
        {
            Id = s.ID,
            AcademicYearDescription = s.AcademicYear != null ? s.AcademicYear.Description : string.Empty,
            StudyYear = s.StudyYear,
            EnrollmentDate = s.EnrollmentDate,
            IsRenewal = s.IsRenewal,
            TuitionFee = s.TuitionFee,
            IsDeleted = s.IsDeleted
        });

        var result = await MyPagedList<SemesterGetAllResponse>.CreateAsync(projectedQuery, request, cancellationToken);
        return result;
    }

    public class SemesterGetAllRequest : MyPagedRequest
    {
        [FromRoute(Name = "studentId")]
        public int StudentId { get; set; }
        public string? Q { get; set; } = string.Empty;
        public string? Status { get; set; } = "all";
    }

    public class SemesterGetAllResponse
    {
        public int Id { get; set; }
        public required string AcademicYearDescription { get; set; }
        public int StudyYear { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsRenewal { get; set; }
        public float TuitionFee { get; set; }
        public bool IsDeleted { get; set; }
    }
}
