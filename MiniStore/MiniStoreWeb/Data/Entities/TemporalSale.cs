using System.ComponentModel.DataAnnotations;

namespace MiniStoreWeb.Data.Entities
{
    public class TemporalSale
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Libro Libro { get; set; }

        [Display(Name = "Cantitdad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Libro == null ? 0 : (decimal)Quantity * Libro.Precio;
    }
}
