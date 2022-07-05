using MiniStoreWeb.Data.Entities;

namespace MiniStoreWeb.Data
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _context;

        public SeedDb(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckBooksAsync();
            //await CheckUsersAsync();
        }

        private async Task CheckBooksAsync()
        {
            if (!_context.Libros.Any())
            {
                for (int i = 0; i < 25; i++)
                {
                _context.Libros.Add(
                     new Libro
                     {
                         Titulo = $"Libro {i}",
                         Editorial = $"Editorial {i}",
                         Descripcion = $"Descripcion {i}",
                         Precio = 55000,
                         Stock = 10
                     });
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
