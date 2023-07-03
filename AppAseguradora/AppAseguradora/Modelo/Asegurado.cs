using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppAseguradora.Modelo
{
    public class Asegurado
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "El campo cedula es obligatorio.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El campo cedula debe contener exactamente 10 números.")]
        public string cedula { get; set; }


        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El nombre debe contener solo letras.")]
        public string nombre { get; set; }


        [Required(ErrorMessage = "El campo telefono es obligatorio.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El campo telefono debe contener exactamente 10 números.")]
        public string telefono { get; set;}

        [Required(ErrorMessage = "El campo edad es obligatorio.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "La edad debe contener solo números.")]
        [Range(18, 99, ErrorMessage = "La edad debe estar entre 18 y 99.")]
        public int edad { get; set; }

        [ForeignKey("Seguros")]
        public int idSeguro { get; set; }
        public Seguro Seguro { get; set; }

       

    }
}
