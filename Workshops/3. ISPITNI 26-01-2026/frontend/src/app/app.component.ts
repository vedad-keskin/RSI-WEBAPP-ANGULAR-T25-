import {Component} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  standalone: false
})
export class AppComponent {
  languages = [
    {code: 'bs', label: 'Bosanski'},
    {code: 'en', label: 'English'}
  ];

  title = 'RS1 - 2024-25 - template 1 ';

  constructor(private translate: TranslateService) {
    // Postavi default jezik
    this.translate.setDefaultLang('bs');
    this.translate.use('bs');
  }

  changeLanguage(lang: string): void {
    this.translate.use(lang); // Promjena jezika u hodu
  }
}
