import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {
  StudentGetByIdEndpointService, StudentGetByIdResponse
} from '../../../../endpoints/student-endpoints/student-get-by-id-endpoint.service';

@Component({
  selector: 'app-student-semesters',
  standalone: false,

  templateUrl: './student-semesters.component.html',
  styleUrl: './student-semesters.component.css'
})
export class StudentSemestersComponent implements OnInit {

  studentId: any;
  // student:any;
  student: StudentGetByIdResponse | null = null;

  constructor(
    private route: ActivatedRoute,
    private studentGetByIdEndpointService:StudentGetByIdEndpointService
  ) {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));
  }

  ngOnInit(): void {

    this.fetchStudent();

    }


  private fetchStudent() {

    this.studentGetByIdEndpointService.handleAsync(this.studentId).subscribe({
      next: (student) => {

        this.student = student;

      },
      error: (error) => console.error('Error loading city data', error),
    });


  }
}
