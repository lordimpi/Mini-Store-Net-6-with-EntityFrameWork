using MiniStoreWeb.common;
using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Helpers;

namespace MiniStoreWeb.Data
{
    public class SeedDb
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(ApplicationDbContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckBooksAsync();
            await CheckRolesAsync();
            await CheckUserAsync("2020", "Santiago", "Acuña", "snt@yopmail.com", "321 777 5042", "Carrera 8i", UserType.Admin);
            await CheckUserAsync("1010", "user", "user", "user@yopmail.com", "343 475 8899", "Carrera user", UserType.User);

        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
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
