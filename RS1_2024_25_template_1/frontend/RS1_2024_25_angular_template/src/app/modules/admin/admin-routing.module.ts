import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AdminLayoutComponent} from './admin-layout/admin-layout.component';
import {DestinationComponent} from './destination/destination.component';
import {DashboardComponent} from './dashboard/dashboard.component';
import {ReservationComponent} from './reservation/reservation.component';
import {AdminErrorPageComponent} from './admin-error-page/admin-error-page.component';
import {Cities1Component} from './cities1/cities1.component';
import {Cities1EditComponent} from './cities1/cities1-edit/cities1-edit.component';
import {Cities2EditComponent} from './cities2/cities2-edit/cities2-edit.component';
import {Cities2Component} from './cities2/cities2.component';
import {Cities3Component} from './cities3/cities3.component';
import {Cities3EditComponent} from './cities3/cities3-edit/cities3-edit.component';
import {MyChatComponent} from '../shared/mychat/mychat.component';
import {
  RxjsSubjectMessageExampleComponent
} from './rxjs-subject-message-example/rxjs-subject-message-example.component';
import {StudentsComponent} from './students/students.component';
import {StudentEditComponent} from './students/student-edit/student-edit.component';

//komentar
const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardComponent},
      {path: 'cities1', component: Cities1Component},
      {path: 'cities2', component: Cities2Component},
      {path: 'cities3', component: Cities3Component},
      {path: 'cities1/new', component: Cities1EditComponent},
      {path: 'cities2/new', component: Cities2EditComponent},
      {path: 'cities3/new', component: Cities3EditComponent},
      {path: 'cities1/edit/:id', component: Cities1EditComponent},
      {path: 'cities2/edit/:id', component: Cities2EditComponent},
      {path: 'cities3/edit/:id', component: Cities3EditComponent},
      {path: 'students', component: StudentsComponent},
      {path: 'students/edit/:id', component: StudentEditComponent},
      {path: 'destination', component: DestinationComponent},
      {path: 'order', component: ReservationComponent},
      {path: 'chat', component: MyChatComponent},
      {path: 'rxjs-subject-message-example', component: RxjsSubjectMessageExampleComponent},
      {path: '**', component: AdminErrorPageComponent} // Default ruta
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
