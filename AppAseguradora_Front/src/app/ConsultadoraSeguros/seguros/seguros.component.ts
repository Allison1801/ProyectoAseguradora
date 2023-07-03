import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AseguradoraService } from '../services/aseguradora.service';
import { Seguros } from '../interfaces/seguros.interfaces';


@Component({
  selector: 'app-seguros',
  templateUrl: './seguros.component.html',
  styleUrls: ['./seguros.component.css']
})
export class SegurosComponent implements OnInit{

  fileToUpload: File | null = null;
  

  formulario: FormGroup;
  excelData: any[] = [];

  _seguros : Seguros[] = [];
   ObjSeguro!:Seguros;
    nombre: string = "";
    suma: number=0;
    prima:number=0;
    id:number=0;
    cliente! : Seguros;


    nuevoSeguro!: Seguros;

  constructor(private aseguradoraService : AseguradoraService, private formBuilder: FormBuilder){
    this.formulario = this.formBuilder.group({
      nombre: ['', [Validators.required, Validators.pattern('[A-Za-z ]*')]],
      suma: ['', [Validators.required, Validators.pattern('[0-9]*')]],
      prima: ['', [Validators.required, Validators.pattern('[0-9]*')]]
    
    });
  }

    ngOnInit(): void {
      this.obtenerSeguros();
    }
 

      obtenerSeguros() {
        this.aseguradoraService.getListseguros().subscribe(data => {
          this._seguros = data;
        });
      }

      EliminarSeguros(id:number){
        this.aseguradoraService.deleteSeguros(id).subscribe(data =>{
          this._seguros = this._seguros.filter(seguro => seguro.id !== id);
          alert('Seguro eliminado correctamente');
         
        },
        error => {
          console.error(error);
        })
      }

      CrearSeguros(){
          this.ObjSeguro = {
            id:0,
            nombre:this.nombre,
            suma:this.suma,
            prima: this.prima
          }
          this.aseguradoraService.postSeguros(this.ObjSeguro).subscribe(
            data => {
              this.ObjSeguro = data;
              alert('Seguro registrado correctamente');
              console.log(data);
              this.obtenerSeguros();
            },
            error =>{
              console.error('Error en la solicitud:', error);
            }
          );
          this.limpiarFormulario();
      }

      ModificarSeguros(){
        this.ObjSeguro = {
          id:this.id,
          nombre:this.nombre,
          suma:this.suma,
          prima: this.prima
        }
             this.aseguradoraService.putSeguros(this.ObjSeguro).subscribe(
               data =>{
               
                 this.obtenerSeguros();
                 alert('Seguro modificado correctamente');
                 this.cerrarPopupEditar();
          
                },
               error =>{
                console.error("Error en la solicitud:",error);
                });
               
      }
  
      abrirPopup(){
        document.querySelector('.popup')?.classList.add('open-popup')
      }
      cerrarPopup(){
        document.querySelector('.popup')?.classList.remove('open-popup')
      }

      abrirPopupEditar(cliente: Seguros){
        this.id = cliente.id;
        this.nombre = cliente.nombre;
        this.suma = cliente.suma;
        this.prima = cliente.prima;
        document.querySelector('.popupEditar')?.classList.add('open-popupEditar')
        
        
      }
      cerrarPopupEditar(){
        document.querySelector('.popupEditar')?.classList.remove('open-popupEditar')
      }


      limpiarFormulario(){
        this.formulario.reset();
      }

      

  
      handleFileInput(event: any) {
        this.fileToUpload = event.target.files[0];
      }
    
      uploadFile() {
        if (this.fileToUpload) {
          this.aseguradoraService.uploadExcel(this.fileToUpload).subscribe(
            response => {
              console.log(response); // Imprimir la respuesta en la consola
              // Mostrar el mensaje de respuesta en tu frontend
              alert(response);
            },
            error => {
              this.cerrarPopup();
              // Mostrar el mensaje de error en tu frontend
              alert('Archivo subido correctamente');
              this.obtenerSeguros();
             
            }
          );
        }
      }

  
}




