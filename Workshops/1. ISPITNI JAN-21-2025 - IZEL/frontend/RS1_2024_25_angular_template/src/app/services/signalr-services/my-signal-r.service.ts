import {Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {MyConfig} from '../../my-config';
import {LoginTokenDto} from '../auth-services/dto/login-token-dto';

@Injectable({providedIn: 'root'})
export class MySignalRService {
  private hubConnection!: signalR.HubConnection;

  // Pokretanje SignalR konekcije
  startConnection() {
    let loginTokenDto: LoginTokenDto = JSON.parse(localStorage.getItem('my-auth-token') || '{}');
    const authToken = loginTokenDto.token;

    if (!authToken) {
      console.error('No auth token found, SignalR connection not started.');
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${MyConfig.api_address}/mysginalr-hub-path?my-auth-token=${authToken}`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch((err) => console.error('Error while connecting SignalR:', err));
  }

  // Zaustavljanje SignalR konekcije
  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .then(() => console.log('SignalR connection stopped'))
        .catch((err) => console.error('Error while stopping SignalR connection:', err));
    }
  }

  // Dodavanje listenera za primanje poruka
  myClientMethod1(callback: (message: string) => void) {
    this.hubConnection.on('myClientMethod1', (data: string) => {
      console.log('Message received:', data);
      callback(data);
    });
  }

  // Slanje poruke serveru
  myServerHubMethod1(toUser: string, message: string) {
    this.hubConnection
      .invoke('MyServerHubMethod1', toUser, message)
      .then(() => console.log('Message sent successfully'))
      .catch((err) => console.error('Error while sending message:', err));
  }
}
