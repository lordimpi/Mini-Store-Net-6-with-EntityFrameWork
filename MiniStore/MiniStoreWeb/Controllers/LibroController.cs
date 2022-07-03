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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Libro libro = await _libroService.GetLibroByIdService(id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _libroService.GetLibroByIdService(id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Libro libro)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Libro mlibro = await _libroService.GetLibroByIdService(id);
                    mlibro.Editorial = libro.Editorial;
                    mlibro.Titulo = libro.Titulo;
                    mlibro.Descripcion = libro.Descripcion;
                    mlibro.Precio = libro.Precio;
                    mlibro.Path = libro.Path;

                    await _libroService.UpdateLibroService(mlibro);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(libro);
        }
    }
}
