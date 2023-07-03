using AppAseguradora.Datos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using AppAseguradora.Modelo;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Text.Json;

namespace AppAseguradora.Controllers
{
    [ApiController]
    [Route("api/Seguros")]

    public class SegurosController : ControllerBase
    {
       
        string nombre;
        double suma;
        double prima;
       


        private readonly AppDBContext _dbContext;

        public SegurosController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        


        }

        //MostrarSeguros
        [HttpGet]
        public async Task<IActionResult> MostrarSeguros()
        {
            try
            {
                var seguro = await _dbContext.Seguros.ToListAsync();
                return Ok(seguro);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Error al listar los seguros" + ex.Message);
            }

        }

        [HttpGet("nombre")]
        public async Task<IActionResult> SegurosPorNombre(string nombre)
        {
            try
            {
                var seguro = await _dbContext.Seguros.Where(s => s.nombre == nombre).ToListAsync();
                return Ok(seguro);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Error al listar los seguros por nombre" + ex.Message);
            }

        }

        [HttpGet("id")]
        public async Task<IActionResult> SegurosPorId(int id)
        {
            try
            {
                var seguro = await _dbContext.Seguros.Where(s => s.id == id).ToListAsync();
                return Ok(seguro);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Error al listar los seguros por id" + ex.Message);
            }

        }








        //CrearSeguros
        [HttpPost]
        public async Task<IActionResult> CrearSeguros([FromBody] Seguro seguro)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devuelve un error de validación
                return BadRequest(ModelState);
            }

            try
            {
                var Objseguro = await _dbContext.Seguros.FirstOrDefaultAsync(u => u.nombre == seguro.nombre);


                if (Objseguro == null)
                {

                    _dbContext.Seguros.Add(seguro);
                    await _dbContext.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    throw new InvalidOperationException("Seguro ya fue registrado");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al registrar seguro.", ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModificarSeguro([FromBody] Seguro seguro, int id)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devuelve un error de validación
                return BadRequest(ModelState);
            }
            try
            {
                //encontrar el objeto
                Seguro Objseguro = await _dbContext.Seguros.FindAsync(id);

                if (Objseguro == null)
                {

                    throw new InvalidOperationException("El seguro no existe");
                }
                Objseguro.nombre = seguro.nombre;
                Objseguro.prima = seguro.prima;
                Objseguro.suma = seguro.suma;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al modificar seguro.", ex);
            }


        }

        [HttpDelete]
        public async Task<IActionResult> EliminarSeguros(int cod_seguro)
        {
            try
            {
                Seguro Objseguro = await _dbContext.Seguros.FindAsync(cod_seguro);

                if (Objseguro == null)
                {

                    throw new InvalidOperationException("El seguro no existe");
                }

                _dbContext.Remove(Objseguro);
                await _dbContext.SaveChangesAsync();
                return Ok();

            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al eliminar el seguro.", ex);
            }
        }


        [HttpPost]
        [Route("api/archivos")]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo)
        {

            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se proporcionó ningún archivo");
            }

            // Leer el archivo de Excel utilizando EPPlus
            using (var package = new ExcelPackage(archivo.OpenReadStream()))
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelWorksheet worksheet = null;
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    if (sheet.Name == "datos")
                    {
                        worksheet = sheet;
                        break;
                    }
                }

                if (worksheet != null)
                {
                    try
                    {
                        // Recorrer las filas del archivo y guardar los datos en la base de datos
                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                        {
                            string nombre = worksheet.Cells[row, 2].Value?.ToString();
                            string primaString = worksheet.Cells[row, 3].Value?.ToString();
                            string sumaString = worksheet.Cells[row, 4].Value?.ToString();
                            Console.Write(nombre);


                            // Validar que los valores de prima y suma sean números enteros válidos
                            int prima;
                            int suma;
                            if (!int.TryParse(primaString, out prima) || !int.TryParse(sumaString, out suma))
                            {

                                throw new InvalidOperationException("Valores de prima y/o suma inválidos");
                            }

                            // Crear el objeto Seguros
                            Seguro seguros = new Seguro
                            {
                                nombre = nombre,
                                prima = prima,
                                suma = suma
                            };

                            // Guardar el objeto Seguros en la base de datos
                            var seguroObj = await _dbContext.Seguros.FirstOrDefaultAsync(s => s.nombre == seguros.nombre);
                            if (seguroObj == null)
                            {
                                _dbContext.Seguros.Add(seguros);
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                throw new InvalidOperationException("Seguro ya registrado");
                            }
                        }

                        return Ok("Archivo subido correctamente");
                    }
                    catch (InvalidOperationException ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                else
                {
                    return BadRequest("La hoja de cálculo 'datos' no se encontró en el archivo");
                }
            }
        }

    }

    }

