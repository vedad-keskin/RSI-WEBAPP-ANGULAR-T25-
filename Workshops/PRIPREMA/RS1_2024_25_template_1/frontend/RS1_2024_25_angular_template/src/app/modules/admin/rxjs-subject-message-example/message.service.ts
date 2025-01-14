import {Injectable} from '@angular/core';
import {startWith, Subject} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private messageSubject = new Subject<string>();

  // Observable sa operatorima
  message$ = this.messageSubject
    .asObservable()
    .pipe(
      startWith("Mo")
    );

  // Emitovanje poruke
  sendMessage(message: string): void {
    this.messageSubject.next(message);
  }
}
