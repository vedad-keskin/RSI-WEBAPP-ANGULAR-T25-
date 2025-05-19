import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  StudentGetByIdEndpointService, StudentGetByIdResponse
} from '../../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';
import {MySnackbarHelperService} from '../../../../shared/snackbars/my-snackbar-helper.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  AcademicYearGetAllEndpoint
} from '../../../../../endpoints/academic-year-endpoints/academic-year-get-all-endpoint.service';
import {
  StudentUpdateOrInsertRequest
} from '../../../../../endpoints/student-endpoints/student-update-or-insert-endpoint.service';
import {
  SemesterUpdateOrInsertEndpoint,
  SemesterUpdateOrInsertRequest
} from '../../../../../endpoints/semester-endpoints/semester-update-or-insert-endpoint.service';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent implements OnInit {

  // Nase varijable
  studentId: any;
  student:StudentGetByIdResponse | null = null;
  academicYears:any;

  semesterForm: FormGroup;
  loggedInUserId:any;


  constructor(  private route: ActivatedRoute,
                private studentGetByIdEndpointService:StudentGetByIdEndpointService,
                private snackbar: MySnackbarHelperService,
                private router: Router,
                private fb: FormBuilder,
                private academicYearGetAllService:AcademicYearGetAllEndpoint,
                private semesterUpdateOrInsertService:SemesterUpdateOrInsertEndpoint
  ) {

    this.studentId = route.snapshot.params['id'];

    const authData = localStorage.getItem('my-auth-token');

    if(authData){

      const JSONAuth = JSON.parse(authData);

      this.loggedInUserId = JSONAuth.myAuthInfo.userId;

    }


    this.semesterForm = this.fb.group({
      academicYearId:[1 , [Validators.required]],
      studentId:[this.studentId , [Validators.required]],
      recordedById:[this.loggedInUserId , [Validators.required]],
      date:[ new Date() , [Validators.required]],
      yearOfStudy:[null , [Validators.required]],
      price:[null , [Validators.required, Validators.min(50), Validators.max(2000)  ]],
      renewal:[false , [Validators.required]],
    });

  }
  ngOnInit(): void {

       this.fetchStudent();
       this.fetchAcademicYears();

    }


  private fetchStudent() {
    this.studentGetByIdEndpointService.handleAsync(this.studentId).subscribe({
      next: (data) => {

        this.student = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching student. Please try again.', 5000);
        console.error('Error fetching student:', err);
      }
    });
  }

  saveSemester() {

    if (this.semesterForm.invalid) return;

    const semeserData: SemesterUpdateOrInsertRequest = {
      ...this.semesterForm.value,
    };

    this.semesterUpdateOrInsertService.handleAsync(semeserData).subscribe({
      next: () => {

        this.router.navigate(['admin/students/semesters/',this.studentId]);
      },
      error: (error) => {
        this.snackbar.showMessage('Error saving semester. Please try again.', 5000);
        console.error('Error saving semester', error);
      },
    });


  }

  private fetchAcademicYears() {

    this.academicYearGetAllService.handleAsync().subscribe({
      next: (data) => {

        this.academicYears = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching academic years. Please try again.', 5000);
        console.error('Error fetching academic years:', err);
      }
    });

  }
}
