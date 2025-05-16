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
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  StudentUpdateOrInsertRequest
} from '../../../../../endpoints/student-endpoints/student-update-or-insert-endpoint.service';
import {
  SemesterUpdateOrInsertEndpoint,
  SemesterUpdateOrInsertRequest
} from '../../../../../endpoints/semester-endpoints/semester-update-or-insert-endpoint.service';
import {
  SemesterGetByStudentIdEndpoint
} from '../../../../../endpoints/semester-endpoints/semester-get-by-student-id-endpoint.service';

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
  loggedInUserId:any;
  semesters:any;


  semesterForm: FormGroup;

  constructor(private route:ActivatedRoute,
              private studentGetByIdService:StudentGetByIdEndpointService,
              private snackbar: MySnackbarHelperService,
              private router: Router,
              private dialog: MatDialog,
              private AcademicYearGetAllService:AcademicYearGetAllEndpoint,
              private fb: FormBuilder,
              private semesterUpdateOrInsertService:SemesterUpdateOrInsertEndpoint,
              private SemesterGetByStudentIdService:SemesterGetByStudentIdEndpoint
              ) {
    this.studentId = route.snapshot.params['id'];

    const authData = localStorage.getItem('my-auth-token');

    if (authData) {
      const JSONAuth = JSON.parse(authData);
      this.loggedInUserId = JSONAuth.myAuthInfo?.userId ?? 0;
    }

    this.semesterForm = this.fb.group({
        studentId: [this.studentId, [Validators.required]],
        recordedById: [this.loggedInUserId, [Validators.required]],
        academicYearId: [1, [Validators.required]],
        date:[new Date(), [Validators.required]],
        yearOfStudy: [null, [Validators.required]],
        price: [null, [Validators.required, Validators.min(50), Validators.max(2000)   ]],
        renewal: [false, [Validators.required]],
    });

  }


    ngOnInit(): void {
        this.fetchStudent();
        this.fetchAcademyYears();
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

  saveSemester() {

    if (this.semesterForm.invalid) return;

    const semesterData: SemesterUpdateOrInsertRequest = {
      ...this.semesterForm.value,
    };

    this.semesterUpdateOrInsertService.handleAsync(semesterData).subscribe({
      next: () => {
        this.snackbar.showMessage('Semester added succesfully.', 5000);
        //this.router.navigate(['/admin/students/semesters/', this.studentId]);
      },
      error: (error) => {
        this.snackbar.showMessage('Error adding semester. Please try again.', 5000);
        console.error('Error adding semester', error);
      },
    });


  }

  YearChanged($event: any) {

    const YearOfStudy : number = parseInt($event.target.value,10); // decimalni 10, binarni 2, 8 octal

    if(this.semesters.some((x:any) => x.yearOfStudy == YearOfStudy)){

      // SET PRICE TO 400
      // SET RENEWAL TO TRUE
      this.semesterForm.patchValue({
        price: 400,
        renewal: true,
      });

    }else{
      this.semesterForm.patchValue({
        price: 1800,
        renewal: false,
      });
    }



  }

  private fetchSemesters() {

    this.SemesterGetByStudentIdService.handleAsync(this.studentId).subscribe({
      next: (data) => {

        this.semesters = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching academic years. Please try again.', 5000);
        console.error('Error fetching academic years:', err);
      }
    });


  }
}
