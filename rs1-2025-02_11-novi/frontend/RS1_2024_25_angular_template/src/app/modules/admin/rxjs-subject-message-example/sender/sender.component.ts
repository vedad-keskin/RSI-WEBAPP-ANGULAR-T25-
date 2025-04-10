import {Component} from '@angular/core';
import {MessageService} from '../message.service';

@Component({
  selector: 'app-sender',
  standalone: false,

  templateUrl: './sender.component.html',
  styleUrl: './sender.component.css'
})
export class SenderComponent {
  message: string = '';

  constructor(private messageService: MessageService) {
  }

  onKeyup(event: KeyboardEvent): void {
    const input = event.target as HTMLInputElement;
    this.message = input.value;
    this.messageService.sendMessage(this.message); // Emituje poruku tokom unosa
  }


}
