import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SegurosComponent } from './ConsultadoraSeguros/seguros/seguros.component';
import { AseguradosComponent } from './ConsultadoraSeguros/asegurados/asegurados.component';
import { ConsultasComponent } from './ConsultadoraSeguros/consultas/consultas.component';

const routes: Routes = [

  {
    path:"",
    component:ConsultasComponent,
    pathMatch:'full',
  },

  {
    path:"seguros",
    component:SegurosComponent,
    pathMatch:'full',
  },
  {
    path:"Asegurados",
    component:AseguradosComponent,
    pathMatch:"full"
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
