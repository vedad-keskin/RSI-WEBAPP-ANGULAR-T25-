import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  StudentGetByIdEndpointService, StudentGetByIdResponse
} from '../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';
import {CityGetAll1Response} from '../../../../endpoints/city-endpoints/city-get-all1-endpoint.service';
import {
  SemesterGetAllEndpoint,
  SemesterGetAllResponse
} from '../../../../endpoints/semester-endpoints/semester-get-all-endpoint.service';
import {tap} from 'rxjs/operators';

@Component({
  selector: 'app-student-semesters',
  standalone: false,

  templateUrl: './student-semesters.component.html',
  styleUrl: './student-semesters.component.css'
})
export class StudentSemestersComponent implements OnInit {

  studentId: any;
  // student:any;
  student: StudentGetByIdResponse | null = null;
  semesters: SemesterGetAllResponse[] = [];
  displayedColumns: string[] = ['id', 'academicYear' ,'yearOfStudy', 'renewal', 'winterSemester', 'recordedBy'];


  constructor(
    private route: ActivatedRoute,
    private studentGetByIdEndpointService:StudentGetByIdEndpointService,
    private semesterGetAllEndpoint:SemesterGetAllEndpoint,
    private router:Router
  ) {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));
  }

  ngOnInit(): void {

    this.fetchStudent();
    this.fetchSemesters();


    }


  private fetchStudent() {

    this.studentGetByIdEndpointService.handleAsync(this.studentId).subscribe({
      next: (student) => {

        this.student = student;

      },
      error: (error) => console.error('Error loading city data', error),
    });


  }

  private fetchSemesters() {

    this.semesterGetAllEndpoint.handleAsync(this.studentId)
      .pipe(tap(x => console.log("fetched: " + x.length)))
      .subscribe({
        next: (data) => (this.semesters = data),
        error: (err) => console.error('Error fetching semesters:', err)
      });

  }

  openNewSemesterDialog(studentId: any) {

    this.router.navigate(['/admin/semesters/new/', studentId]);

  }
}
