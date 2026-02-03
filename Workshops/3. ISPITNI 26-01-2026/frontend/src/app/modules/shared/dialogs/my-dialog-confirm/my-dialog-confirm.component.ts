import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';

@Component({
  selector: 'app-my-dialog-confirm',
  standalone: false,

  templateUrl: './my-dialog-confirm.component.html',
  styleUrl: './my-dialog-confirm.component.css'
})
export class MyDialogConfirmComponent {
  constructor(
    public dialogRef: MatDialogRef<MyDialogConfirmComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { title: string; message: string, confirmButtonText: string }
  ) {
  }

  onConfirm(): void {
    this.dialogRef.close(true); // Vraća true kada korisnik potvrdi
  }

  onCancel(): void {
    this.dialogRef.close(false); // Vraća false kada korisnik otkaže
  }
}
