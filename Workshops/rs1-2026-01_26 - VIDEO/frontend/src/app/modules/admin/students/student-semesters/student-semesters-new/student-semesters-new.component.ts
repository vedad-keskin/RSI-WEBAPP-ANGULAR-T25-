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
import {ActivatedRoute, Router} from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {MatTableDataSource} from '@angular/material/table';
import {MySnackbarHelperService} from '../../../../shared/snackbars/my-snackbar-helper.service';
import {
  SemesterUpdateOrInsertEndpointService, SemesterUpdateOrInsertRequest
} from '../../../../../endpoints/semester-endpoints/semester-update-or-insert-endpoint.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  AcademicYearLookupEndpointService
} from '../../../../../endpoints/lookup-endpoints/academic-year-lookup-endpoint.service';
import {
  StudentUpdateOrInsertRequest
} from '../../../../../endpoints/student-endpoints/student-update-or-insert-endpoint.service';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent implements OnInit {

  studentId:number = 0;

  dataSource: MatTableDataSource<SemesterGetAllResponse> = new MatTableDataSource<SemesterGetAllResponse>();
  semesterForm: FormGroup;

  academicYears:any;

  constructor(
    private semesterGetAllService:SemesterGetAllEndpointService,
    private router: Router,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private snackbar: MySnackbarHelperService,
    private semesterUpdateOrInsertService:SemesterUpdateOrInsertEndpointService,
    private fb: FormBuilder,
    private academicYearLookupService:AcademicYearLookupEndpointService

  ) {


    this.semesterForm = this.fb.group({

      tuitionFee: [{value:null, disabled:true}],
      isRenewal: [{value:null, disabled:true}],
      academicYearId: [1, [Validators.required]],
      studyYear: [null, [Validators.required]],
      enrollmentDate: [new Date(), [Validators.required]],

    });

  }

  ngOnInit(): void {

    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    this.fetchSemesters();
    this.fetchAcademicYears();
  }

  fetchSemesters(filter: string = '', page: number = 1, pageSize: number = 5): void {
    this.semesterGetAllService.handleAsync(this.studentId,
      {
        pageNumber: page,
        pageSize: 100,

      }
    ).subscribe({
      next: (data) => {
        this.dataSource = new MatTableDataSource<SemesterGetAllResponse>(data.dataItems);
      },
      error: (err) => {
        console.error('Error fetching semesters:', err);
      },

    });
  }

  saveSemester() {

    if (this.semesterForm.invalid) return;

    const semesterData: SemesterUpdateOrInsertRequest = {
      ...this.semesterForm.value,
    };

    this.semesterUpdateOrInsertService.handleAsync(this.studentId,semesterData).subscribe({
      next: () => {


        this.router.navigate(['/admin/students', this.studentId, 'semesters']);

      },
      error: (error) => {
        console.error('Error saving semester', error);

        this.snackbar.showMessage(error.error.split('...')[0], 5000);

        // this.snackbar.showMessage('Student has already enrolled in that academic year', 5000);

      },
    });


  }

  private fetchAcademicYears() {

    this.academicYearLookupService.handleAsync().subscribe({
      next: (data) => {

        this.academicYears = data;

      },
      error: (err) => {
        console.error('Error fetching academic years:', err);
      },

    });

  }

  changedYear($event: any) {

    var studyYear = Number($event.target.value);
    var studyYear1 = parseInt($event.target.value);

    var count = this.dataSource.data.filter(x => x.studyYear == studyYear).length;

    if(count == 0){

      this.semesterForm.patchValue({

        tuitionFee: 1800,
        isRenewal: false,

      })

    }else if(count == 1){

       this.semesterForm.patchValue({

         tuitionFee: 400,
         isRenewal: true,

       })

    }else{

      this.semesterForm.patchValue({

        tuitionFee: 500,
        isRenewal: true,

      })

    }


  }


}
