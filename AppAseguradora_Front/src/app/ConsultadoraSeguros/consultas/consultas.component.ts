import { Component, OnInit } from '@angular/core';
import { AseguradoraService } from '../services/aseguradora.service';
import { Asegurados } from '../interfaces/asegurados.interfaces';
import { Seguros } from '../interfaces/seguros.interfaces';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-consultas',
  templateUrl: './consultas.component.html',
  styleUrls: ['./consultas.component.css']
})
export class ConsultasComponent{
  _asegurados: Asegurados [] =[]; 
  _seguros: Seguros [] = [];
  idSeguro: number = 0;
  cedula: string ="";
  nuevoForm: FormGroup;
  
  constructor( private aseguradoraService: AseguradoraService, private formBuilder: FormBuilder ){
    
    this.nuevoForm = this.formBuilder.group({
      codigo: ['', Validators.required],
      cedula: ['', Validators.required],
    });

  }

 
  obtenerAseguradosPorId(id:number) {
    this.aseguradoraService.getListapersonaSeguros(id).subscribe(data => {
      this._asegurados = data;
      this.limpiarFormulario();
      console.log(this._asegurados)
    });
  }

  obtenerSegurosPorCedula(cedula: string){
    this.aseguradoraService.getListasegurosxpersona(cedula).subscribe(data => {
      this._asegurados = data;
      this.limpiarFormulario();
      console.log(this._asegurados)
    });
  }

  limpiarFormulario(){
    this.nuevoForm.reset();
  }

}
