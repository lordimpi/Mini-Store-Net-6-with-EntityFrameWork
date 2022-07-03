using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Helpers;
using MiniStoreWeb.Services;
using Vereyon.Web;
using static MiniStoreWeb.Helpers.ModalHelper;

namespace MiniStoreWeb.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILibroService _libroService;
        private readonly IFlashMessage _flashMessage;

        public LibroController(ILibroService libroService, IFlashMessage flashMessage)
        {
            _libroService = libroService;
            _flashMessage = flashMessage;
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

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            Libro libro = await _libroService.GetLibroByIdService(id);
            try
            {
                await _libroService.DeleteLibroService(id);
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el libro porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Libro());
            }
            else
            {
                Libro libro = await _libroService.GetLibroByIdService(id);
                if (libro == null)
                {
                    return NotFound();
                }

                return View(libro);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        await _libroService.CreateLibroService(libro);
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        libro.Codigo = id;
                        await _libroService.UpdateLibroService(libro);
                        _flashMessage.Info("Registro actualizado.");
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                    return View(libro);
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(libro);
                }
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllBooks", _libroService.GetLibrosService()) });
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", libro) });
        }
    }
}
