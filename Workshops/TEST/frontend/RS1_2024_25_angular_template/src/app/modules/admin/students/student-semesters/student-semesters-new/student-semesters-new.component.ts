import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  StudentGetByIdEndpointService
} from '../../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';
import {MySnackbarHelperService} from '../../../../shared/snackbars/my-snackbar-helper.service';
import {MatDialog} from '@angular/material/dialog';
import {
  AcademicYearGetAllEndpoint
} from '../../../../../endpoints/academic-year-endpoints/academic-year-get-all-endpoint.service';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent implements OnInit {

  studentId:number = 0;
  student:any;
  academyYears:any;

  constructor(private route:ActivatedRoute,
              private studentGetByIdService:StudentGetByIdEndpointService,
              private snackbar: MySnackbarHelperService,
              private router: Router,
              private dialog: MatDialog,
              private AcademicYearGetAllService:AcademicYearGetAllEndpoint
              ) {
    this.studentId = route.snapshot.params['id'];
  }


    ngOnInit(): void {
        this.fetchStudent();
        this.fetchAcademyYears();
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

  private fetchAcademyYears() {

    this.AcademicYearGetAllService.handleAsync().subscribe({
      next: (data) => {

        this.academyYears = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching academic years. Please try again.', 5000);
        console.error('Error fetching academic years:', err);
      }
    });

  }
}
