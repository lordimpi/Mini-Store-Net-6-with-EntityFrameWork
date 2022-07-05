using System.ComponentModel.DataAnnotations;

namespace MiniStoreWeb.Models
{
    public class AddBookImageViewModel
    {
        public int BookId { get; set; }

        [Display(Name = "Foto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public IFormFile ImageFile { get; set; }
    }
}
