import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AseguradosComponent } from './asegurados/asegurados.component';
import { SegurosComponent } from './seguros/seguros.component';
import { ConsultasComponent } from './consultas/consultas.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';





@NgModule({
  declarations: [
    AseguradosComponent,
    SegurosComponent,
    ConsultasComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  
  ]
})
export class ConsultadoraSegurosModule { }
