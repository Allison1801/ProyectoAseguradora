using AppAseguradora.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using AppAseguradora.Modelo;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace AppAseguradora.Controllers
{
    [ApiController]
    [Route("api/Asegurados")]
    public class AseguradosController : ControllerBase

    {
        int id;
        string nombre;
        string cedula;
        string telefono;
        int edad;
        int idSeguro;
        

        private readonly AppDBContext _dbContext;
        public AseguradosController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        //MostrarAseguros
        [HttpGet]
        public async Task<IActionResult> MostrarSeguros()
        {
            try
            {
                var asegurado = await _dbContext.Asegurados.ToListAsync();
                return Ok(asegurado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest("Error al listar los aseguros" + ex.Message);
            }

        }


        [HttpGet("cedula")]
        public async Task<IActionResult> ObtenerAsegurados(string cedula)
        {
            try
            {
                //busca por la cedula
                var asegurado = await _dbContext.Asegurados.Where(s => s.cedula==cedula).ToListAsync();
                return Ok(asegurado);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error, la cedula no coincide", ex);
            }
        }


        [HttpGet("id")]
        public async Task<IActionResult> ObtenerAsegurados(int id_seguro)
        {
            try
            {
                var asegurados = await _dbContext.Asegurados.Where(a => a.id == id_seguro).ToListAsync();

                return Ok(asegurados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CrearAsegurado([FromBody] Asegurado asegurado)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devuelve un error de validación
                return BadRequest(ModelState);
            }
            try
            {
                var seguroObj = await _dbContext.Seguros.FirstOrDefaultAsync(s => s.nombre == asegurado.Seguro.nombre);

                if (seguroObj != null)
                {
                    Asegurado aseguradoObj = await _dbContext.Asegurados.FirstOrDefaultAsync(a => (a.Seguro.nombre == seguroObj.nombre && a.nombre == asegurado.nombre));
                    if (aseguradoObj == null)
                    {
                        asegurado.Seguro = seguroObj;
                        _dbContext.Asegurados.Add(asegurado);
                        await _dbContext.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Ya se encuentra asegurado");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Seguro ya registrado");
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al registrar seguro.", ex);
            }
        }



        //Editar usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAsegurado(int id, [FromBody] Asegurado asegurado)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devuelve un error de validación
                return BadRequest(ModelState);
            }

            try
            {
                var aseguradoActualizado = await _dbContext.Asegurados.FirstOrDefaultAsync(a => a.id == id);

                if (aseguradoActualizado != null)
                {
                    var seguroObj = await _dbContext.Seguros.FirstOrDefaultAsync(s => s.nombre == asegurado.Seguro.nombre);
                    if (seguroObj != null)
                    {
                        aseguradoActualizado.cedula = asegurado.cedula;
                        aseguradoActualizado.nombre = asegurado.nombre;
                        aseguradoActualizado.telefono = asegurado.telefono;
                        aseguradoActualizado.edad = asegurado.edad;
                        _dbContext.Asegurados.Update(aseguradoActualizado);
                        await _dbContext.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        // El seguro no existe
                        throw new InvalidOperationException("Seguro no registrado");
                    }
                }
                else
                {
                    // El asegurado no existe
                    return NotFound();
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Error al actualizar asegurado.", ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarSeguros(int id)
        {
            try
            {
                Asegurado ObjAsegurado = await _dbContext.Asegurados.FindAsync(id);

                if (ObjAsegurado == null)
                {

                    throw new InvalidOperationException("El seguro no existe");
                }

                _dbContext.Remove(ObjAsegurado);
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
                            string cedula = worksheet.Cells[row, 2].Value?.ToString();
                            string nombre = worksheet.Cells[row, 3].Value?.ToString();
                            string telefono = worksheet.Cells[row, 4].Value?.ToString();
                            string edadString = worksheet.Cells[row, 5].Value?.ToString();
                            string idSeguroString = worksheet.Cells[row, 6].Value?.ToString();
                            Console.Write(nombre);


                            // Validar que los valores de prima y suma sean números enteros válidos
                            int edad;
                            int idSeguro;
                            if (!int.TryParse(edadString, out edad) || !int.TryParse(idSeguroString, out idSeguro))
                            {

                                throw new InvalidOperationException("Valores de edad y/o idSeguro inválidos");
                            }

                            // Crear el objeto Seguros
                            Asegurado asegurado = new Asegurado
                            {
                                cedula = cedula,
                                nombre = nombre,
                                telefono = telefono,
                                edad = edad,
                                idSeguro = idSeguro
                            };

                            // Guardar el objeto Seguros en la base de datos
                            var AseguradoObj = await _dbContext.Asegurados.FirstOrDefaultAsync(s => s.nombre == asegurado.nombre);
                            if (AseguradoObj == null)
                            {
                                _dbContext.Asegurados.Add(asegurado);
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
