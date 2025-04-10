import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MyPageProgressbarService {

  private isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$ = this.isLoadingSubject.asObservable();

  private minDuration = 2000; // Minimalno trajanje progress bara u milisekundama
  private showStartTime: number | null = null;

  show(): void {
    this.showStartTime = Date.now();
    this.isLoadingSubject.next(true);
  }

  hide(): void {
    if (this.showStartTime) {
      const elapsedTime = Date.now() - this.showStartTime;
      const remainingTime = this.minDuration - elapsedTime;

      if (remainingTime > 0) {
        // Ako je prošlo manje od 2 sekunde, odložite skrivanje
        setTimeout(() => {
          this.isLoadingSubject.next(false);
          this.showStartTime = null;
        }, remainingTime);
      } else {
        // Ako je prošlo dovoljno vremena, odmah sakrij
        this.isLoadingSubject.next(false);
        this.showStartTime = null;
      }
    } else {
      this.isLoadingSubject.next(false);
    }
  }
}
