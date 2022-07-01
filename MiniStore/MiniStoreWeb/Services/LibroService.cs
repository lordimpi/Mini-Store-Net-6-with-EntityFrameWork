using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Repositories;

namespace MiniStoreWeb.Services
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _libroRepository;

        public LibroService(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        public async Task<bool> CreateLibroService(Libro libro)
        {
            return await _libroRepository.CreateLibro(libro);
        }

        public async Task<bool> DeleteLibroService(int codigo)
        {
            return await _libroRepository.DeleteLibro(codigo);
        }

        public async Task<Libro> GetLibroByIdService(int codigo)
        {
            return await _libroRepository.GetLibroById(codigo);
        }

        public List<Libro> GetLibrosService()
        {
            return _libroRepository.GetLibros();
        }

        public async Task<bool> UpdateLibroService(Libro libro)
        {
            return await _libroRepository.UpdateLibro(libro);
        }
    }
}
