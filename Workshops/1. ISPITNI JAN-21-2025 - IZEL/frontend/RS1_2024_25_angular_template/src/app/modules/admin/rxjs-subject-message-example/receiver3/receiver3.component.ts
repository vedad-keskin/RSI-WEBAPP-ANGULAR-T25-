import {Component, OnInit} from '@angular/core';
import {MessageService} from '../message.service';
import {map, mergeMap, tap} from 'rxjs/operators';
import {StudentGetAllEndpointService} from '../../../../endpoints/student-endpoints/student-get-all-endpoint.service';

@Component({
  selector: 'app-receiver3',
  standalone: false,

  templateUrl: './receiver3.component.html',
  styleUrl: './receiver3.component.css'
})
export class Receiver3Component implements OnInit {
  dataArray: any[] = [];

  constructor(private messageService: MessageService, private apiClient: StudentGetAllEndpointService) {
  }

  ngOnInit(): void {
    this.messageService.message$
      .pipe(
        tap(v => console.log(v)),
        mergeMap(message => this.apiClient.handleAsync({q: message, pageNumber: 1, pageSize: 100})),
        map(respone => respone.dataItems),
        map(studentsArray => studentsArray.map(s => s.firstName + " " + s.lastName)),
      )
      .subscribe((firstNamesArray) => {
        this.dataArray = firstNamesArray;
      });
  }
}
