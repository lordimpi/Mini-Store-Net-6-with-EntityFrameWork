using System.ComponentModel.DataAnnotations;

namespace MiniStoreWeb.Data.Entities
{
    public class BookImage
    {
        public int Id { get; set; }

        public Libro Libro { get; set; }

        [Display(Name = "Foto")]
        public string ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == string.Empty
            ? $"https://localhost:44384/images/noimage.png"
            //? $"https://impistoreshop.azurewebsites.net/images/noimage.png"
            : $"https://localhost:44384/images/books/{ImageId}";
    }
}
