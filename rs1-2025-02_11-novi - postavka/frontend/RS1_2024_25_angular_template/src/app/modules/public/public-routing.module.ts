import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PublicLayoutComponent} from './public-layout/public-layout.component';
import {HomeComponent} from './home/home.component';
import {AboutComponent} from './about/about.component';
import {BlogComponent} from './blog/blog.component';
import {ContactUsComponent} from './contact-us/contact-us.component';
import {TravelsComponent} from './travels/travels.component';

const routes: Routes = [
  {
    path: '', component: PublicLayoutComponent, children: [
      {path: '', redirectTo: 'home', pathMatch: 'full'},
      {path: 'home', component: HomeComponent},
      {path: 'about', component: AboutComponent},
      {path: 'blog', component: BlogComponent},
      {path: 'contact-us', component: ContactUsComponent},
      {path: 'travels', component: TravelsComponent},
      {path: '**', redirectTo: 'home', pathMatch: 'full'}  // Default ruta koja vodi na public
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
