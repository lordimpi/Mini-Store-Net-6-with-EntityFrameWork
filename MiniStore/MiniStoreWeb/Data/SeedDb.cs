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

            await CheckUserAsync("3030", "Jhojana", "Villota", "jhojanavillota@hotmail.com", "321 545 5042", "Carrera la piedra", UserType.Admin);
            await CheckUserAsync("2020", "Santiago", "Acuña", "snt-26@hotmail.com", "321 777 5042", "Carrera 8i", UserType.Admin);
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
                _context.Libros.Add(
                new Libro
                {
                    Titulo = $"El Coronel",
                    Editorial = $"linfier",
                    Descripcion = $"Alice Brown, una jovencita de 23 años. Un poco alocada, soñadora e impulsiva. Sueña con terminar sus estudios universitarios y darle a su padre una gran vida. Toma la decisión más difícil, que es mudarse a otra ciudad, dejando a su padre solo, en aquel lejano pueblo. ",
                    Precio = 55000,
                    Stock = 10,
                });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"El Engaño",
                     Editorial = $"linfier",
                     Descripcion = $"Un accidente fue el inicio de todo... ella sin desearlo se vio envuelta en una telaraña de mentiras, cuando quiso salir de ella... ya era demasiado tarde... el engaño ya estaba bien plantado y la verdad podía destruirla y a las personas que amaba. Nada de lo que parece puede ser real",
                     Precio = 75000,
                     Stock = 5
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Enamorada por casualidad",
                     Editorial = $"Itzel",
                     Descripcion = $"La vida de Katherine se basa en la familia, estudios y amigos, excluye toda clase de romances debido a una experiencia fatídica con Sebastián. Todo esto cambia poco a poco cuando aparecen dos chicos que entre ellos son dos polos opuestos a hacerla dudar respecto al amor.",
                     Precio = 59000,
                     Stock = 15
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Un Lobo en el Bosque",
                     Editorial = $"Itzel",
                     Descripcion = $"Jenny solo es una chica que ha vivido toda su vida en la ciudad pero, todo esto cambia cuando su abuela aparece de la nada para llevarla consigo, al bosque. Ella acepta, sin saber las consecuencias que traería sus actos.",
                     Precio = 99000,
                     Stock = 32
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Jefe por Contrato",
                     Editorial = $"Itzel",
                     Descripcion = $"Jade Bomwer tiene 23 años su mejor amiga trabaja en una de las mejores empresas todo comienza cuando visita por primera vez a su mejor amiga a su trabajo , ella no se esperaba encontrar con un hombre guapo , alto, sexy y ojos verdes con una sonrisa muy pero muy sexi .",
                     Precio = 89000,
                     Stock = 22
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Mi Primer Amor",
                     Editorial = $"Itzel",
                     Descripcion = $"Joel Sólo quiero decir que NO la olvidé, pero tampoco quiero recordar esa etapa de mi niñez..",
                     Precio = 89000,
                     Stock = 12
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Los Secretos de Venus",
                     Editorial = $"Itzel",
                     Descripcion = $"He descubierto que las personas no son más que una capa tras otra de secretos. Crees que las conoces, que las entiendes, pero sus motivos siempre permanecen ocultos, enterrados en sus corazones",
                     Precio = 49000,
                     Stock = 12
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Matar a la Reina",
                     Editorial = $"Hat",
                     Descripcion = $"Las alegres Navidades de Micaela Bravo se ven interrumpidas cuando, con solo doce años, alguien a quién creía de su familia le arranca la infancia acabando con lo que más quiere. Todos sus seres queridos son asesinados sin piedad y, ella, ultrajada y agredida hasta tal punto que sus agresores",
                     Precio = 79000,
                     Stock = 22
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"La Dama del Nilo",
                     Editorial = $"Hat",
                     Descripcion = $"Mil seiscientos años antes que Cleopatra, reinó en Egipto Hatshepsut, una mujer extraordinaria por su inteligencia y su belleza, y también por ser la primera de la historia que gobernó en un mundo dominado por los hombres. Según la tradición secular, los faraones de Egipto solo podían reinar si se casaban con una mujer de sangre real que, mediante el matrimonio, otorgaba al hombre la condición de",
                     Precio = 109000,
                     Stock = 43
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Dejame Amarte",
                     Editorial = $"Hat",
                     Descripcion = $"Kenan tenía la vida perfecta: una esposa a la cual amaba y, un matrimonio Esperaba ansioso el nacimiento de los mellizos de ambos sexo, pero un accidente le hará ver que vivió en una mentira...",
                     Precio = 69000,
                     Stock = 13
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"¿Y si te Vuelvo a Encontrar?",
                     Editorial = $"Hat",
                     Descripcion = $"Sé que estas triste y a la vez me odias por dejarte, y no de una, sino de dos maneras, pero amor ¿Y si te vuelvo a encontrar? ¿,Me amarías como la primera vez? Liam Evans nunca creyó que la persona que más amaba, lo dañaría tanto, al punto de matarlo en vida...",
                     Precio = 69000,
                     Stock = 11
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Eres Inolvidable",
                     Editorial = $"Hat",
                     Descripcion = $"Glenda es una mujer risueña que le haya siempre un lado bueno a todo, sin embargo después de terminar su relación con Cristian, ahora esta con su nuevo novio... ",
                     Precio = 49000,
                     Stock = 11
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"La Dama y El Grial I: El misterio de la orden",
                     Editorial = $"Az",
                     Descripcion = $"El caballero Guillaume de Saissac es testigo del asesinato de su padre en París. Así descubre que es miembro importante de una orden antigua y secreta que tiene dos misiones: Proteger al Grial...",
                     Precio = 49000,
                     Stock = 11
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Ambicion Oscura",
                     Editorial = $"Az",
                     Descripcion = $"Libro 2: trilogía Deseo. Rodrigo Ferraioli, el nuevo jefe, todo este tiempo había creído que Camille se había suicidado; sin embargo, eso no era así...",
                     Precio = 88000,
                     Stock = 21
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Sangre de Luna",
                     Editorial = $"Az",
                     Descripcion = $"Amely nunca ha tenido miedo del lobo que ronda en su aldea. Pero ¿Qué sucedería si ahora ella supiera la verdad que se esconde tras esos ojos amarillos?...",
                     Precio = 56000,
                     Stock = 11
                 });
                _context.Libros.Add(
                 new Libro
                 {
                     Titulo = $"Efimero",
                     Editorial = $"Az",
                     Descripcion = $"Julie perdió al amor de su vida hace tres años, sin embargo, la realidad sale a la luz, la verdadera razón de la muerte de Raúl. ...",
                     Precio = 66000,
                     Stock = 12
                 });
                await _context.SaveChangesAsync();
            }
        }
    }
}
