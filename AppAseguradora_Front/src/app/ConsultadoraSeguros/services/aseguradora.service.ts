import { Injectable } from '@angular/core';
import { Asegurados } from '../interfaces/asegurados.interfaces';
import { Seguros } from '../interfaces/seguros.interfaces';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AseguradoraService {
  
   _seguros: Seguros [] = [];
  _asegurados: Asegurados [] =[]; 
  fileToUpload: File | null = null;
  excelData: any[] = [];


  constructor(private httpClient : HttpClient){

  }

  
    //Seguros
    getListseguros() {
      return this.httpClient.get<Seguros[]>(`${environment.apiUrl}/seguros`)
    }

    postSeguros(segur: Seguros){
      console.log(segur);
      return this.httpClient.post<Seguros>(`${environment.apiUrl}/Seguros`,segur); 
    }

    getListsegurosxnombre(nombre: string){
      const params = new HttpParams()
      .set('nombre',nombre)
      return this.httpClient.get<Seguros[]>(`${environment.apiUrl}/seguros/nombre?`,{params:params})
    }

    getSegurosPorId(id: number){
      const params = new HttpParams()
      .set('id',id)
      return this.httpClient.get<Seguros[]>(`${environment.apiUrl}/seguros/id?`,{params:params})
    }

    deleteSeguros(id : number){
        const params = new HttpParams()
        .set('cod_seguro',id)
        return this.httpClient.delete<Seguros[]>(`${environment.apiUrl}/Seguros?`,{params:params});
    }

    putSeguros(Seguros: Seguros){
      const params = new HttpParams()
      .set('id', Seguros.id.toString());
      return this.httpClient.put<Seguros>(`${environment.apiUrl}/Seguros`,Seguros,{params:params});
    }

    //Asegurados
    getListasegurado() {
      return this.httpClient.get<Asegurados[]>(`${environment.apiUrl}/Asegurados`)
       
   }
      postAsegurados(asegurados: Asegurados){
        console.log(asegurados);
        return this.httpClient.post<Asegurados>(`${environment.apiUrl}/Asegurados`,asegurados); 
      }

    getListapersonaSeguros(id: number) {
        const params = new HttpParams()
        .set('id_seguro',id)
        return this.httpClient.get<Asegurados[]>(`${environment.apiUrl}/Asegurados/id?`,{params:params})
    }
    getListasegurosxpersona(cedula: string) {
        const params = new HttpParams()
        .set('cedula',cedula)
        return this.httpClient.get<Asegurados[]>(`${environment.apiUrl}/Asegurados/cedula?`,{params:params})
    }

    deleteAsegurados(id : number){
      const params = new HttpParams()
      .set('id',id)
      return this.httpClient.delete<Asegurados[]>(`${environment.apiUrl}/Asegurados?`,{params:params});
  }

  putAsegurados(Asegurados: Asegurados){
    console.log(Asegurados)
    return this.httpClient.put<Asegurados>(`${environment.apiUrl}/Asegurados/${Asegurados.id}`,Asegurados);
    
  }


  handleFileInput(files: FileList): void {
    this.fileToUpload = files.item(0);
  }

  uploadExcel(file: File) {
    const formData = new FormData();
    formData.append('archivo', file);

    return this.httpClient.post<any>('https://localhost:7021/api/Seguros/api/archivos',formData);
  }

  handleFileInputAsegurados(files: FileList): void {
    this.fileToUpload = files.item(0);
  }

  uploadExcelAsegurados(file: File) {
    const formData = new FormData();
    formData.append('archivo', file);

    return this.httpClient.post<any>('https://localhost:7021/api/Asegurados/api/archivos',formData);
  }


}

