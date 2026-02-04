import {Component, OnDestroy, OnInit} from '@angular/core';
import {
  ChatUserGetEndpointService,
  ChatUserGetResponse
} from '../../../endpoints/chat-endpoints/chat-user-get-endpoint.service';
import {MySignalRService} from '../../../services/signalr-services/my-signal-r.service';

@Component({
  selector: 'app-mychat',
  templateUrl: './mychat.component.html',
  styleUrls: ['./mychat.component.css'],
  standalone: false
})
export class MyChatComponent implements OnInit, OnDestroy {
  users: ChatUserGetResponse[] = [];
  selectedUser: ChatUserGetResponse | null = null;
  messages: { from: string; content: string }[] = [];
  newMessage: string = '';
  searchTerm: string = '';
  userType: string = '';

  constructor(
    private chatUserService: ChatUserGetEndpointService,
    private signalRService: MySignalRService
  ) {
  }

  ngOnInit(): void {
    this.fetchUsers();
    this.signalRService.startConnection();

    // Listener za primanje poruka
    this.signalRService.myClientMethod1((message: string) => {
      if (this.selectedUser) {
        this.messages.push({from: this.selectedUser.firstName, content: message});
      }
    });
  }

  ngOnDestroy(): void {
    this.signalRService.stopConnection();
  }

  fetchUsers(): void {
    this.chatUserService
      .handleAsync({userType: this.userType, searchTerm: this.searchTerm})
      .subscribe((response) => {
        this.users = response;
      });
  }

  selectUser(user: ChatUserGetResponse): void {
    this.selectedUser = user;
    this.messages = []; // Reset poruka pri promjeni korisnika
  }

  sendMessage(): void {
    if (this.selectedUser && this.newMessage.trim()) {
      // Slanje poruke preko SignalR
      this.signalRService.myServerHubMethod1(this.selectedUser.email, this.newMessage);

      // Dodavanje poruke u lokalni prikaz
      this.messages.push({from: 'Me', content: this.newMessage});
      this.newMessage = ''; // Resetiranje polja za unos
    }
  }
}
