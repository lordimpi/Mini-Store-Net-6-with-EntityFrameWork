using Microsoft.EntityFrameworkCore;
using MiniStoreWeb.Data;
using MiniStoreWeb.Data.Entities;

namespace MiniStoreWeb.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LibroRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> CreateLibro(Libro libro)
        {
            _dbContext.Add(libro);
            return await _dbContext.SaveChangesAsync() > 0;


        }

        public async Task<bool> DeleteLibro(int? codigo)
        {
            Libro libro = await _dbContext.Libros.FindAsync(codigo);
            _dbContext.Remove(libro);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<Libro> GetLibroById(int? codigo)
        {
            Libro libro = await _dbContext.Libros
                .Include(l => l.BookImages)
                .FirstOrDefaultAsync(l => l.Codigo == codigo);
            return libro;
        }

        public List<Libro> GetLibros()
        {
            List<Libro> libros = _dbContext.Libros
                .Include(l => l.BookImages)
                .ToList();
            return libros;
        }

        public async Task<bool> UpdateLibro(Libro libro)
        {
            _dbContext.Entry(libro).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
