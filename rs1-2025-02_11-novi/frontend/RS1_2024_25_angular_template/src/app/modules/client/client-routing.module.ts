import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ReservationComponent} from './reservation/reservation.component';

const routes: Routes = [
  {path: 'reservation', component: ReservationComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule {
}
