using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.Models.SharedTables;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul1_Auth;
using RS1_2024_25.API.Data.Models.TenantSpecificTables.Modul2_Basic;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.Api;
using RS1_2024_25.API.Services;
using System.ComponentModel.DataAnnotations.Schema;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.SemesterGetAllEndpoint;
using static RS1_2024_25.API.Endpoints.StudentEndpoints.StudentGetAllEndpoint;

namespace RS1_2024_25.API.Endpoints.StudentEndpoints;

// Endpoint za vraćanje liste studenata s filtriranjem i paginacijom
[Route("students")]
//[MyAuthorization(isAdmin: true, isManager: false)]
public class SemesterGetAllEndpoint(ApplicationDbContext db) : MyEndpointBaseAsync
    .WithRequest<SemesterGetAllRequest>
    .WithResult<MyPagedList<SemesterGetAllResponse>>
{
    [HttpGet("{studentId}/semesters/filter")]
    public override async Task<MyPagedList<SemesterGetAllResponse>> HandleAsync([FromQuery] SemesterGetAllRequest request, CancellationToken cancellationToken = default)
    {
        // Osnovni upit za semesters
        var query = db.Semesters
            .Include(x => x.AcademicYear)
            .Include(x => x.Student)
            .Include(x => x.RecordedBy)
            .Where(x => x.StudentId == request.StudentId)
                   .AsQueryable();


        //if (!string.IsNullOrWhiteSpace(request.Status))
        //{
        //    switch (request.Status.ToLower()) 
        //    {


        //        case "active":
        //            query = query.Where(x => !x.IsDeleted);
        //            break;

        //        case "deleted":
        //            query = query.Where(x => x.IsDeleted);
        //            break;

        //        case "all":
                    
        //            break;



        //    }


        //}

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if( request.Status.ToLower() == "active")
            {
                query = query.Where(x => !x.IsDeleted);

            }
            else if (request.Status.ToLower() == "deleted")
            {

                query = query.Where(x => x.IsDeleted);

            }


        }


        // Primjena filtera po imenu, prezimenu, student broju ili državi
        if (!string.IsNullOrWhiteSpace(request.Q))
        {
            query = query.Where(s =>
                s.AcademicYear.Description.Contains(request.Q)
            );
        }




        // Projektovanje u DTO tip za rezultat
        var projectedQuery = query.Select(s => new SemesterGetAllResponse
        {
            ID = s.ID,
            AcademicYearDescription = s.AcademicYear != null ? s.AcademicYear.Description : "",
            StudyYear = s.StudyYear,
            EnrollmentDate = s.EnrollmentDate,
            IsRenewal = s.IsRenewal,
            TuitionFee = s.TuitionFee,
            IsDeleted = s.IsDeleted,
            
        });

        // Kreiranje paginiranog rezultata
        var result = await MyPagedList<SemesterGetAllResponse>.CreateAsync(projectedQuery, request, cancellationToken);

        return result;
    }

    // DTO za zahtjev s podrškom za paginaciju i filtriranje
    public class SemesterGetAllRequest : MyPagedRequest
    {
        [FromRoute(Name="studentId")]
        public int StudentId { get; set; }
        public string? Q { get; set; } = string.Empty; // Tekstualni upit za pretragu
        public string? Status { get; set; } = "all"; // Tekstualni upit za pretragu


    }

    // DTO za odgovor
    public class SemesterGetAllResponse
    {
        public required int ID { get; set; }

        public string? AcademicYearDescription { get; set; }

        public int StudyYear { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public float TuitionFee { get; set; }
        public bool IsRenewal { get; set; }
        public bool IsDeleted { get; set; }
    }
}
