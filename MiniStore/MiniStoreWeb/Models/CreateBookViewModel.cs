using System.ComponentModel.DataAnnotations;

namespace MiniStoreWeb.Models
{
    public class CreateBookViewModel
    {
        public int Codigo { get; set; }

        [Display(Name = "Editorial")]
        [MaxLength(100, ErrorMessage = "El máximo numero de caractéres es de {0}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Editorial { get; set; }

        [Display(Name = "Título")]
        [MaxLength(50, ErrorMessage = "El máximo numero de caractéres es de {0}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Titulo { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Précio")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Precio { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Stock { get; set; }

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }
    }
}
