import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {AdminRoutingModule} from './admin-routing.module';
import {DashboardComponent} from './dashboard/dashboard.component';
import {DestinationComponent} from './destination/destination.component';
import {AdminLayoutComponent} from './admin-layout/admin-layout.component';
import {ReservationComponent} from './reservation/reservation.component';
import {AdminErrorPageComponent} from './admin-error-page/admin-error-page.component';
import {Cities1Component} from './cities1/cities1.component';
import {Cities1EditComponent} from './cities1/cities1-edit/cities1-edit.component';
import {FormsModule} from '@angular/forms';
import {SharedModule} from '../shared/shared.module';
import {Cities2EditComponent} from './cities2/cities2-edit/cities2-edit.component';
import {Cities2Component} from './cities2/cities2.component';
import {MatButton} from "@angular/material/button";
import {
  MatCell,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderRow,
  MatRow,
  MatTable,
  MatTableModule
} from "@angular/material/table";
import {MatPaginator} from '@angular/material/paginator';
import {MatFormField, MatFormFieldModule} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {MatSortModule} from '@angular/material/sort';
import {MatIconModule} from '@angular/material/icon';
import {MatOption, MatSelect} from "@angular/material/select";
import {MatCard} from '@angular/material/card';
import {Cities3Component} from './cities3/cities3.component';
import {Cities3EditComponent} from './cities3/cities3-edit/cities3-edit.component';
import {MatProgressSpinner} from "@angular/material/progress-spinner";
import {
  RxjsSubjectMessageExampleComponent
} from './rxjs-subject-message-example/rxjs-subject-message-example.component';
import {SenderComponent} from './rxjs-subject-message-example/sender/sender.component';
import {Receiver2Component} from './rxjs-subject-message-example/receiver2/receiver2.component';
import { Receiver1Component } from './rxjs-subject-message-example/receiver1/receiver1.component';
import { Receiver3Component } from './rxjs-subject-message-example/receiver3/receiver3.component';
import { StudentsComponent } from './students/students.component';
import { StudentEditComponent } from './students/student-edit/student-edit.component';
import { StudentSemestersComponent } from './students/student-semesters/student-semesters.component';
import { StudentSemestersNewComponent } from './students/student-semesters/student-semesters-new/student-semesters-new.component';


@NgModule({
  declarations: [
    DashboardComponent,
    DestinationComponent,
    AdminLayoutComponent,
    ReservationComponent,
    AdminErrorPageComponent,
    Cities1Component,
    Cities2Component,
    Cities3Component,
    Cities1EditComponent,
    Cities2EditComponent,
    Cities3EditComponent,
    SenderComponent,
    Receiver2Component,
    RxjsSubjectMessageExampleComponent,
    SenderComponent,
    Receiver1Component,
    Receiver3Component,
    StudentsComponent,
    StudentEditComponent,
    StudentSemestersComponent,
    StudentSemestersNewComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    SharedModule,
    MatButton,
    MatTable,
    MatHeaderCell,
    MatCell,
    MatHeaderRow,
    MatRow,
    MatPaginator,
    MatFormField,
    MatInput,
    MatIconModule,
    MatColumnDef,
    MatTableModule,
    MatSortModule,
    MatFormFieldModule,
    MatSelect,
    MatOption,
    MatCard,
    MatProgressSpinner,
    // Omogućava pristup svemu što je eksportovano iz SharedModule
  ],
  providers: []
})
export class AdminModule {
}
