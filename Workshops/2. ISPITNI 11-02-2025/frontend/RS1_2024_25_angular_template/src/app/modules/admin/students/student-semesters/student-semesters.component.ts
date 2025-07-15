import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  StudentGetByIdEndpointService, StudentGetByIdResponse
} from '../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';
import {MySnackbarHelperService} from '../../../shared/snackbars/my-snackbar-helper.service';
import {MatDialog} from '@angular/material/dialog';
import {
  SemesterGetAllByStudentIdEndpoint
} from '../../../../endpoints/semster-endpoints/semester-get-all-by-student-id-endpoint.service';

@Component({
  selector: 'app-student-semesters',
  standalone: false,

  templateUrl: './student-semesters.component.html',
  styleUrl: './student-semesters.component.css'
})
export class StudentSemestersComponent implements OnInit {

  // Nase globalne varijable

  studentId:number = 0;
  //student:any;
  student: StudentGetByIdResponse | null = null;
  semesters:any;
  displayedColumns: string[] = ['id', 'academicYear', 'yearOfStudy', 'renewal','dateOfEnrollment','recordedBy' ];



  constructor(private route: ActivatedRoute,
              private studentGetByIdService:StudentGetByIdEndpointService,
              private snackbar: MySnackbarHelperService,
              private router: Router,
              private dialog: MatDialog,
              private semesterGetAllByStudentIdService:SemesterGetAllByStudentIdEndpoint
              ) {

    this.studentId = this.route.snapshot.params['id'];
  }

  ngOnInit(): void {
        this.fetchStudent();
        this.fetchSemesters();
    }


  private fetchStudent() {

    this.studentGetByIdService.handleAsync(this.studentId).subscribe({
      next: (data) => {

        this.student = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching student. Please try again.', 5000);
        console.error('Error fetching student:', err);
      }
    });

  }

  private fetchSemesters() {

    this.semesterGetAllByStudentIdService.handleAsync(this.studentId).subscribe({
      next: (data) => {

        this.semesters = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching semesters. Please try again.', 5000);
        console.error('Error fetching semesters:', err);
      }
    });


  }

  goToNewSemester() {
    this.router.navigate(['/admin/students/semesters/new', this.studentId]);

  }
}
