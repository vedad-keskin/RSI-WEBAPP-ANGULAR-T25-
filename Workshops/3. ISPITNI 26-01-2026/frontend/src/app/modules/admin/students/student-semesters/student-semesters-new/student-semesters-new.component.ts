import {Component, OnInit} from '@angular/core';
import {
  SemesterGetAllEndpointService, SemesterGetAllResponse
} from '../../../../../endpoints/semester-endpoints/semester-get-all-endpoint.service';
import {
  SemesterDeleteEndpointService
} from '../../../../../endpoints/semester-endpoints/semester-delete-endpoint.service';
import {
  SemesterRestoreEndpointService
} from '../../../../../endpoints/semester-endpoints/semester-restore-endpoint.service';
import {Router} from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {
  SemesterUpdateOrInsertEndpointService, SemesterUpdateOrInsertRequest
} from '../../../../../endpoints/semester-endpoints/semester-update-or-insert-endpoint.service';
import {MatTableDataSource} from '@angular/material/table';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  AcademicYearLookupEndpointService
} from '../../../../../endpoints/lookup-endpoints/academic-year-lookup-endpoint.service';
import {
  StudentUpdateOrInsertRequest
} from '../../../../../endpoints/student-endpoints/student-update-or-insert-endpoint.service';
import {MySnackbarHelperService} from '../../../../shared/snackbars/my-snackbar-helper.service';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent implements OnInit  {



  dataSource: MatTableDataSource<SemesterGetAllResponse> = new MatTableDataSource<SemesterGetAllResponse>();
  semesterForm: FormGroup;
  academicYears:any;


  constructor(
    private semesterGetService: SemesterGetAllEndpointService,
    private semesterUpdateOrInsertService: SemesterUpdateOrInsertEndpointService,
    private academicYearLookupService:AcademicYearLookupEndpointService,
    private fb: FormBuilder,
    private router: Router,
    private dialog: MatDialog,
    private snackbar: MySnackbarHelperService,

  ) {

    this.semesterForm = this.fb.group({

      tuitionFee: [{value: null , disabled: true}],
      isRenewal: [{value: null , disabled: true}],
      academicYearId: [1, [Validators.required]],
      studyYear:  [null, [Validators.required]],
      enrollmentDate:  [new Date(), [Validators.required]],

    });


  }

  ngOnInit(): void {
    this.fetchSemesters();
    this.fetchAcademicYears();
  }

  fetchSemesters(filter: string = '', page: number = 1, pageSize: number = 5): void {
    this.semesterGetService.handleAsync(1, {
      pageNumber: page,
      pageSize: 100,
    })

      .subscribe({
        next: (data) => {
          this.dataSource = new MatTableDataSource<SemesterGetAllResponse>(data.dataItems);
        },
        error: (err) => {
          console.error('Error fetching semesters:', err);
        },
        complete: () => {


        }
      });
  }


  saveSemester() {

    if (this.semesterForm.invalid) return;

    const semesterData: SemesterUpdateOrInsertRequest = {
      ...this.semesterForm.value,
    };

    this.semesterUpdateOrInsertService.handleAsync(1,semesterData).subscribe({
      next: () => {
        this.router.navigate(['/admin/students',1,'semesters']);
      },
      error: (error) => {

        this.snackbar.showMessage(error.error.split('..')[0], 5000);


        //alert(errorMessage); // Display to user
      },
    });


  }

  private fetchAcademicYears() {

    this.academicYearLookupService.handleAsync().subscribe({
      next: (data) => {

        this.academicYears = data;

      },
      error: (err) => {

      }
    });

  }

  yearChange($event: any) {

     var studyyear = parseInt( $event.target.value);

     var count = this.dataSource.data
       .filter(x => x.studyYear === studyyear)
       .length;

     if(count == 0){

       this.semesterForm.patchValue({
         tuitionFee : 1800,
         isRenewal : false,

       })

     }else if(count == 1){

       this.semesterForm.patchValue({
         tuitionFee : 400,
         isRenewal : true,

       })

     }else {

       this.semesterForm.patchValue({
         tuitionFee : 500,
         isRenewal : true,

       })

     }




  }
}
