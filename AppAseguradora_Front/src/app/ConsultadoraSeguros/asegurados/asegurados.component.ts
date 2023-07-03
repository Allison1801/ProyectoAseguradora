import { Component, Input, OnInit } from '@angular/core';
import { AseguradoraService } from '../services/aseguradora.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Asegurados } from '../interfaces/asegurados.interfaces';
import { Seguros } from '../interfaces/seguros.interfaces';

@Component({
  selector: 'app-asegurados',
  templateUrl: './asegurados.component.html',
  styleUrls: ['./asegurados.component.css']
})
export class AseguradosComponent implements OnInit {

  formulario: FormGroup;
  _asegurados: Asegurados [] =[];
  _seguros: Seguros []= [];

  fileToUpload: File | null = null;
  
 
  excelData: any[] = [];


  ObjAsegurados!: Asegurados;
  id: number =0;
  idSeguro: number = 0;
  nombre: string ="";
  cedula: string ="";
  telefono: string="";
  edad: number=0;
  ObjSeguros!: Seguros; 
  nombreS:string="";
  suma:number=0;
  prima:number=0;
  seguro:string="";

 

  
  constructor(private aseguradoraService : AseguradoraService, private formBuilder: FormBuilder) 
   {
    this.formulario = this.formBuilder.group({
      nombre: ['', [Validators.required, Validators.pattern('[A-Za-z ]*')]],
      cedula: ['', [Validators.required, Validators.pattern('[0-9]{0,10}$')]],
      telefono: ['', [Validators.required, Validators.pattern('[0-9]{0,10}$')]],
      edad: ['', [Validators.required, Validators.pattern('^[0-9]*$')]],
      seguro: ['', [Validators.required, Validators.pattern('[A-Za-z ]*')]],
    });
   }


  ngOnInit(): void {
    this.obtenerAsegurados();
  }
  
 

  crearAsegurados(){
     this.aseguradoraService.getListsegurosxnombre(this.seguro).subscribe(data =>{
      this._seguros = data;
       //iguala al objeto para ver lo que hay en el primer elemento del arreglo
       this.ObjSeguros = this._seguros[0]; 
       this.ObjAsegurados = {
         id:0,
         nombre:this.nombre,
         cedula:this.cedula,
         telefono:this.telefono,
         edad:this.edad,
         idSeguro:0,
         Seguro: this.ObjSeguros
      }
       this.aseguradoraService.postAsegurados(this.ObjAsegurados).subscribe(
        (response) => {
          console.log('Solicitud POST exitosa:', response);
          alert('Asegurado creado correctamente');
          this.obtenerAsegurados();
        
        },
        (error) => {
          console.error('Error en la solicitud POST:', error);
        });
       this.limpiarFormulario();
     })
    console.log(this.seguro);
  }

  obtenerAsegurados() {
    this.aseguradoraService.getListasegurado().subscribe(data => {
      this._asegurados = data;
      // console.log(this._asegurados)
    });
  }
 
  obtenerSegurosPorNombre(){
    this.aseguradoraService.getListsegurosxnombre(this.ObjAsegurados.Seguro.nombre).subscribe(
        data=>{
          data= data;
          console.log(data);
        },
        error =>{
          console.error("Error en la solicitud", error);
        }
      )
  }

 

 
  EliminarAsegurados(id:number){
    this.aseguradoraService.deleteAsegurados(id).subscribe(data =>{
      this._asegurados = this._asegurados.filter(asegurado => asegurado.id !== id);
      alert('Asegurado eliminado correctamente');
     
    },
    error => {
      console.error(error);
    })
  }
  

  ModificarAsegurados (){
    this.aseguradoraService.getSegurosPorId(this.idSeguro).subscribe(
      data =>{
        this.ObjSeguros = data[0];
        this.ObjAsegurados = {
          id:this.id,
          nombre:this.nombre,
          cedula:this.cedula,
          telefono:this.telefono,
          edad:this.edad,
          idSeguro:this.idSeguro,
          Seguro: this.ObjSeguros

        }
        this.aseguradoraService.putAsegurados(this.ObjAsegurados).subscribe(data =>
          {
            alert('Asegurado modificado correctamente');
            this.obtenerAsegurados();
            this.cerrarPopupEditar();
          },
         error =>{
          console.error(error);
          }
         )
       
      },
      error =>{
        console.error("Error en la solicitud", error);
      }
    )
  }


   limpiarFormulario(){
    this.formulario.reset();
  }
   
    
   abrirPopup(){

    document.querySelector('.popup')?.classList.add('open-popup')
  }
  cerrarPopup(){
    document.querySelector('.popup')?.classList.remove('open-popup')
  }

  abrirPopupEditar(cliente: Asegurados){
    this.id= cliente.id;
    this.nombre = cliente.nombre;
    this.cedula = cliente.cedula;
    this.edad = cliente.edad;
    this.telefono = cliente.telefono;
    this.idSeguro = cliente.idSeguro;
 
    document.querySelector('.popupEditar')?.classList.add('open-popupEditar')
    
    
  }
  cerrarPopupEditar(){
    document.querySelector('.popupEditar')?.classList.remove('open-popupEditar')
  }

  handleFileInputAsegurados(event: any) {
    this.fileToUpload = event.target.files[0];
  }

  uploadFile() {
    if (this.fileToUpload) {
      this.aseguradoraService.uploadExcelAsegurados(this.fileToUpload).subscribe(
        response => {
          console.log(response); // Imprimir la respuesta en la consola
          // Mostrar el mensaje de respuesta en tu frontend
          alert(response);
        },
        error => {
          this.cerrarPopup();
          // Mostrar el mensaje de error en tu frontend
          alert('Archivo subido correctamente');
          this.obtenerAsegurados();
         
        }
      );
    }
  }


}
