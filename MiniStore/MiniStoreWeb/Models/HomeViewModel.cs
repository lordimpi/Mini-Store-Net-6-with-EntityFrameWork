using MiniStoreWeb.common;
using MiniStoreWeb.Data.Entities;

namespace MiniStoreWeb.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Libro> Libros { get; set; }
        public float Quantity { get; set; }
    }
}
