import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  StudentGetByIdEndpointService,
  StudentGetByIdResponse
} from '../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';
import {
  StudentUpdateOrInsertEndpointService,
  StudentUpdateOrInsertRequest,
  Gender
} from '../../../../endpoints/student-endpoints/student-update-or-insert-endpoint.service';
import {
  MunicipalityLookupEndpointService,
  MunicipalityLookupResponse
} from '../../../../endpoints/lookup-endpoints/municipality-lookup-endpoint.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css'],
  standalone: false,
})
export class StudentEditComponent implements OnInit {
  studentForm: FormGroup;
  studentId: number;
  genders = [
    { id: Gender.Male, name: 'Male' },
    { id: Gender.Female, name: 'Female' },
    { id: Gender.Other, name: 'Other' }
  ];
  municipalities: MunicipalityLookupResponse[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private studentGetByIdService: StudentGetByIdEndpointService,
    private studentUpdateService: StudentUpdateOrInsertEndpointService,
    private municipalityLookupService: MunicipalityLookupEndpointService,
    private snackBar: MatSnackBar
  ) {
    this.studentId = 0;

    this.studentForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      studentNumber: ['', [Validators.required, Validators.pattern('^[A-Z]{2,3}[0-9]{6}$')]],
      gender: [null, [Validators.required]],
      birthMunicipalityId: [null],
      permanentMunicipalityId: [null],
      contactMobilePhone: ['', /*[Validators.pattern('^\\+?[0-9]{7,15}$')]*/],
      contactPrivateEmail: ['', [Validators.email]],
    });
  }

  ngOnInit(): void {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.studentId) {
      this.loadStudentData();
    }
    this.loadMunicipalities();
  }

  loadStudentData(): void {
    this.studentGetByIdService.handleAsync(this.studentId).subscribe({
      next: (student: StudentGetByIdResponse) => {
        this.studentForm.patchValue({
          firstName: student.firstName,
          lastName: student.lastName,
          studentNumber: student.studentNumber,
          gender: this.genders.find(g => g.name === student.gender)?.id,
          birthMunicipalityId: student.birthMunicipalityId,
          permanentMunicipalityId: student.permanentMunicipalityId,
          contactMobilePhone: student.contactMobilePhone,
          contactPrivateEmail: student.contactPrivateEmail,
        });
      },
      error: (error) => {
        this.snackBar.open('Error loading student data. Please try again.', 'Close', { duration: 5000 });
        console.error('Error loading student data', error);
      },
    });
  }

  loadMunicipalities(): void {
    this.municipalityLookupService.handleAsync({}).subscribe({
      next: (municipalities: MunicipalityLookupResponse[]) => {
        this.municipalities = municipalities;
      },
      error: (error) => {
        this.snackBar.open('Error loading municipalities. Please try again.', 'Close', { duration: 5000 });
        console.error('Error loading municipalities', error);
      },
    });
  }

  saveStudent(): void {
    if (this.studentForm.invalid) return;

    const studentData: StudentUpdateOrInsertRequest = {
      id: this.studentId,
      ...this.studentForm.value,
    };

    this.studentUpdateService.handleAsync(studentData).subscribe({
      next: () => {
        this.snackBar.open('Student saved successfully!', 'Close', { duration: 5000 });
        this.router.navigate(['/admin/students']);
      },
      error: (error) => {
        this.snackBar.open('Error saving student. Please try again.', 'Close', { duration: 5000 });
        console.error('Error saving student', error);
      },
    });
  }
}
