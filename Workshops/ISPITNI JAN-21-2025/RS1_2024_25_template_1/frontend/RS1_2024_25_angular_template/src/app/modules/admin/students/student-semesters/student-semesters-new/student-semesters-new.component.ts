import { Component } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-student-semesters-new',
  standalone: false,

  templateUrl: './student-semesters-new.component.html',
  styleUrl: './student-semesters-new.component.css'
})
export class StudentSemestersNewComponent {

  semesterForm: FormGroup;
  studentId: any;

  constructor(
    private route: ActivatedRoute,
    private router:Router,
    private fb: FormBuilder,
  ) {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));

    this.semesterForm = this.fb.group({
      academicYearId: [0, [Validators.required]],
      studentId: [this.studentId, [Validators.required]],
      recordedById: [1, [Validators.required]],
      yearOfStudy: [null, [Validators.required]],
      renewal: [false, [Validators.required]],
      date: [Date.now(), [Validators.required]],
      price: [null, [Validators.required, Validators.min(50), Validators.max(2000)]],
    });

  }

  saveSemester() {

  }


}
