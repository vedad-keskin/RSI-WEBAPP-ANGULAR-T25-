import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PublicLayoutComponent} from './public-layout/public-layout.component';
import {HomeComponent} from './home/home.component';

const routes: Routes = [
  {
    path: '', component: PublicLayoutComponent, children: [
      {path: '', redirectTo: 'home', pathMatch: 'full'},
      {path: 'home', component: HomeComponent},
      {path: '**', redirectTo: 'home', pathMatch: 'full'}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
