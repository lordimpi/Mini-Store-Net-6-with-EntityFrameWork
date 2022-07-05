using MiniStoreWeb.Data.Entities;

namespace MiniStoreWeb.Repositories
{
    public interface ILibroRepository
    {
        List<Libro> GetLibros();
        Task<bool> CreateLibro(Libro libro);
        Task<bool> UpdateLibro(Libro libro);
        Task<bool> DeleteLibro(int? codigo);
        Task<Libro> GetLibroById(int? codigo);
    }
}
