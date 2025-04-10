import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {
  AcademicYearGetAllEndpoint, AcademicYearGetAllResponse
} from '../../../../../endpoints/academic-year-endpoints/academic-year-get-all-endpoint.service';
import {tap} from 'rxjs/operators';
import {
  SemesterUpdateOrInsertEndpoint, SemesterUpdateOrInsertRequest
} from '../../../../../endpoints/semester-endpoints/semester-update-or-insert-endpoint.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import {
  SemesterGetAllEndpoint,
  SemesterGetAllResponse
} from '../../../../../endpoints/semester-endpoints/semester-get-all-endpoint.service';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent implements OnInit {

  semesterForm: FormGroup;
  studentId: any;
  academicYears: AcademicYearGetAllResponse[] = [];
  loggedInUserId:any;

  semesters: SemesterGetAllResponse[] = [];


  constructor(
    private route: ActivatedRoute,
    private router:Router,
    private fb: FormBuilder,
    private academicYearGetAllEndpoint:AcademicYearGetAllEndpoint,
    private semesterUpdateOrInsertEndpoint:SemesterUpdateOrInsertEndpoint,
    private snackBar: MatSnackBar,
    private semesterGetAllEndpoint:SemesterGetAllEndpoint
  ) {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    const authData = localStorage.getItem('my-auth-token');

    if (authData) {
      const parsedAuth = JSON.parse(authData);
      this.loggedInUserId = parsedAuth.myAuthInfo?.userId ?? 0;
    }


    this.semesterForm = this.fb.group({
      academicYearId: [1, [Validators.required]],
      studentId: [this.studentId, [Validators.required]],
      recordedById: [this.loggedInUserId, [Validators.required]],
      yearOfStudy: [null, [Validators.required]],
      renewal: [false, [Validators.required]],
      //date: [Date.now(), [Validators.required]],
      date: [new Date(), [Validators.required]],
      price: [null, [Validators.required, Validators.min(50), Validators.max(2000)]],
    });

  }

  ngOnInit(): void {
    this.fetchAcademicYears();
    this.fetchSemesters();

    // Disable the fields that should not be changed
    this.semesterForm.get('price')?.disable();
    this.semesterForm.get('renewal')?.disable();
  }

  saveSemester() {
    if (this.semesterForm.invalid) return;

    const semesterData: SemesterUpdateOrInsertRequest = {
      ...this.semesterForm.value,
      price: this.semesterForm.get('price')?.value,  // Manually add price when it's disabled
      renewal: this.semesterForm.get('renewal')?.value  // Manually add renewal when it's disabled
    };

    console.log(semesterData);  // Check if 'price' and 'renewal' are included

    this.semesterUpdateOrInsertEndpoint.handleAsync(semesterData).subscribe({
      next: () => {
        this.snackBar.open('Semester saved successfully!', 'Close', { duration: 5000 });
        this.router.navigate([`/admin/semesters/${this.studentId}`]);
      },
      error: (error) => {
        this.snackBar.open('Error saving semester. Please try again.', 'Close', { duration: 5000 });
        console.error('Error saving semester', error);
      },
    });

  }


  private fetchAcademicYears() {
    this.academicYearGetAllEndpoint.handleAsync()
      .pipe(tap(x => console.log("fetched: " + x.length)))
      .subscribe({
        next: (data) => (this.academicYears = data),
        error: (err) => console.error('Error fetching semesters:', err)
      });
  }

  onValueChange($event: any) {

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
    this.semesterGetAllEndpoint.handleAsync(this.studentId)
      .pipe(tap(x => console.log("fetched: " + x.length)))
      .subscribe({
        next: (data) => (this.semesters = data),
        error: (err) => console.error('Error fetching semesters:', err)
      });
  }
}
