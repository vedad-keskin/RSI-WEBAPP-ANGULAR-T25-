import {Component, Inject} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-student-semesters',
  standalone: false,

  templateUrl: './student-semesters.component.html',
  styleUrl: './student-semesters.component.css'
})
export class StudentSemestersComponent {

  // Nase globalne varijable
  studentId:any;


  constructor(
    // copied from student-edit.component.ts
    private route: ActivatedRoute,
  ) {

    this.studentId = this.route.snapshot.params['id'];

  }

}
