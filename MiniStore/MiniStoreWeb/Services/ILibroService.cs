using MiniStoreWeb.Data.Entities;

namespace MiniStoreWeb.Services
{
    public interface ILibroService
    {
        List<Libro> GetLibrosService();
        Task<bool> CreateLibroService(Libro libro);
        Task<bool> UpdateLibroService(Libro libro);
        Task<bool> DeleteLibroService(int codigo);
        Task<Libro> GetLibroByIdService(int codigo);
    }
}
