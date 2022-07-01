using Microsoft.AspNetCore.Mvc;
using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Services;

namespace MiniStoreWeb.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILibroService _libroService;

        public LibroController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        public IActionResult Index()
        {
            List<Libro> libroList = _libroService.GetLibrosService();
            return View(libroList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _libroService.CreateLibroService(libro);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(libro);
        }
    }
}
