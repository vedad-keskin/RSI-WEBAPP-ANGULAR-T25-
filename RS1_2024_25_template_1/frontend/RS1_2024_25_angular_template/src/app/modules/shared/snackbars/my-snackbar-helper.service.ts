import {Injectable} from '@angular/core';
import {MatSnackBar, MatSnackBarRef, TextOnlySnackBar} from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class MySnackbarHelperService {


  constructor(private snackBar: MatSnackBar) {
  }

  /**
   * Prikazuje jednostavan snackbar s porukom.
   * @param message Tekst poruke
   * @param duration Trajanje prikaza u milisekundama (podrazumijevano: 3000)
   */
  showMessage(message: string, duration: number = 3000): MatSnackBarRef<TextOnlySnackBar> {
    return this.snackBar.open(message, undefined, {
      duration: duration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
    });
  }

  /**
   * Prikazuje snackbar s dodatnom opcijom za klik.
   * @param message Tekst poruke
   * @param action Tekst dugmeta (npr. "Undo")
   * @param callback Funkcija koja se poziva kada se klikne na dugme
   * @param duration Trajanje prikaza u milisekundama (podrazumijevano: 5000)
   */
  showMessageWithAction(
    message: string,
    action: string,
    callback: () => void,
    duration: number = 5000
  ): void {
    const snackBarRef = this.snackBar.open(message, action, {
      duration: duration,
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
    });

    snackBarRef.onAction().subscribe(() => {
      callback();
    });
  }
}
