import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {
  StudentGetByIdEndpointService, StudentGetByIdResponse
} from '../../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';
import {MySnackbarHelperService} from '../../../../shared/snackbars/my-snackbar-helper.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  AcademicYearEndpoint
} from '../../../../../endpoints/academic-year-endpoints/academic-year-get-all-endpoint.service';
import {
  StudentUpdateOrInsertRequest
} from '../../../../../endpoints/student-endpoints/student-update-or-insert-endpoint.service';
import {
  SemesterUpdateOrInsertEndpoint,
  SemesterUpdateOrInsertRequest
} from '../../../../../endpoints/semster-endpoints/semester-update-or-insert-endpoint.service';
import {
  SemesterGetAllByStudentIdEndpoint
} from '../../../../../endpoints/semster-endpoints/semester-get-all-by-student-id-endpoint.service';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent implements OnInit {


  studentId:number = 0;
  student: StudentGetByIdResponse | null = null;
  semesterForm: FormGroup;
  academicYears:any;
  loggedInUserId:number = 0;
  semesters:any;


  constructor(private route: ActivatedRoute,
              private studentGetByIdService:StudentGetByIdEndpointService,
              private snackbar: MySnackbarHelperService,
              private fb: FormBuilder,
              private router: Router,
              private academicYearService:AcademicYearEndpoint,
              private semesterUpdateOrInsertService:SemesterUpdateOrInsertEndpoint,
              private semesterGetAllByStudentIdService:SemesterGetAllByStudentIdEndpoint


  ) {

    this.studentId = this.route.snapshot.params['id'];

    const authData = localStorage.getItem('my-auth-token');

    if(authData){

      const JSONAuth = JSON.parse(authData);

      this.loggedInUserId = JSONAuth.myAuthInfo.userId;

    }


    this.semesterForm = this.fb.group({

      academicYearId: [1, [Validators.required]],
      studentId: [this.studentId , [Validators.required]],
      recordedById: [this.loggedInUserId , [Validators.required]],
      dateOfEnrollment: [new Date , [Validators.required]],
      yearOfStudy: [ null , [Validators.required]],
      price: [  {value: null , disabled: true } , [Validators.required, Validators.min(50), Validators.max(2000)]],
      renewal: [ {value: false , disabled: true } , [Validators.required]],

    });

  }



    ngOnInit(): void {
      this.fetchStudent();
      this.fetchAcademicYears();
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


  saveSemester() {

    if (this.semesterForm.invalid) return;

    const semestarData: SemesterUpdateOrInsertRequest = {
      ...this.semesterForm.value,
      price : this.semesterForm.get('price')?.value,
      renewal : this.semesterForm.get('renewal')?.value,
    };

    this.semesterUpdateOrInsertService.handleAsync(semestarData).subscribe({
      next: () => {


        this.router.navigate(['/admin/students/semesters', this.studentId]);
      },
      error: (error) => {
        this.snackbar.showMessage('Error adding semester. Please try again.', 5000);
        console.error('Error adding semester', error);
      },
    });

  }

  private fetchAcademicYears() {

    this.academicYearService.handleAsync().subscribe({
      next: (data) => {

        this.academicYears = data;

      },
      error: (err) => {
        this.snackbar.showMessage('Error fetching academic years. Please try again.', 5000);
        console.error('Error fetching academic years:', err);
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

  YearChanged($event: any) {

    const YearOfStudy: number =  parseInt($event.target.value);

    if(this.semesters.some((x:any) => x.yearOfStudy ==  YearOfStudy)){

      this.semesterForm.patchValue(
        {
          price : 400,
          renewal : true

        })

    }else{

      this.semesterForm.patchValue(
        {
          price : 1800,
          renewal : false

        })

    }


  }
}
