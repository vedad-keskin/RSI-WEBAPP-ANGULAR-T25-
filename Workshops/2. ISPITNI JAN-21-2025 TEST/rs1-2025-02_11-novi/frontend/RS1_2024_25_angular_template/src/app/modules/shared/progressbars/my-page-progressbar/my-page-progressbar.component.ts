import {Component} from '@angular/core';
import {MyPageProgressbarService} from '../my-page-progressbar.service';

@Component({
  selector: 'app-my-page-progressbar',
  standalone: false,

  templateUrl: './my-page-progressbar.component.html',
  styleUrl: './my-page-progressbar.component.css'
})
export class MyPageProgressbarComponent {
  isLoading = false;

  constructor(private progressBarService: MyPageProgressbarService) {
    this.progressBarService.isLoading$.subscribe(isLoading => {
      this.isLoading = isLoading;
    });
  }
}
