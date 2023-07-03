import { Seguros } from "./seguros.interfaces";

export interface Asegurados{
    id:number,
    nombre: string,
    cedula:string,
    telefono:string,
    edad: number,
    idSeguro: number,
    Seguro : Seguros
}