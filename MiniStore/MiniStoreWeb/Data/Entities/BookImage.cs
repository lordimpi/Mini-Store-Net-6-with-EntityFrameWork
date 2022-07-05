using System.ComponentModel.DataAnnotations;

namespace MiniStoreWeb.Data.Entities
{
    public class BookImage
    {
        public int Id { get; set; }

        public Libro Libro { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:44384/images/noimage.png"
            //? $"https://impistoreshop.azurewebsites.net/images/noimage.png"
            : $"https://localhost:44384/images/{ImageId}";
    }
}
