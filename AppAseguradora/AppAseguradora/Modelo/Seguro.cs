using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AppAseguradora.Modelo
{
    public class Seguro
    {

        [Key]
        
        public int id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El nombre debe contener solo letras.")]
        public string nombre { get; set; }

   
        [Required(ErrorMessage = "El campo suma es obligatorio.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Ingrese una suma válida.")]
        public double suma { get; set; }

        [Required(ErrorMessage = "El campo prima es obligatorio.")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Ingrese una prima válida.")]
        public double prima { get; set; }

       





    }
}
