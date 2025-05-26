import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';

@Component({
  selector: 'app-my-dialog-simple',
  standalone: false,

  templateUrl: './my-dialog-simple.component.html',
  styleUrl: './my-dialog-simple.component.css'
})
export class MyDialogSimpleComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { title: string; message: string }) {

  }
}
