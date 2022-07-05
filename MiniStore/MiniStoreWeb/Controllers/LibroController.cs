using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreWeb.Data;
using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Helpers;
using MiniStoreWeb.Models;
using MiniStoreWeb.Services;
using Vereyon.Web;
using static MiniStoreWeb.Helpers.ModalHelper;

namespace MiniStoreWeb.Controllers
{
    public class LibroController : Controller
    {
        private readonly ILibroService _libroService;
        private readonly IFlashMessage _flashMessage;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LibroController(ILibroService libroService, IFlashMessage flashMessage,
            ApplicationDbContext context, IWebHostEnvironment env)
        {
            _libroService = libroService;
            _flashMessage = flashMessage;
            _context = context;
            _env = env;
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

        [NoDirectAccess]
        public async Task<IActionResult> AddImage(int? id)
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

            AddBookImageViewModel model = new()
            {
                BookId = libro.Codigo
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddBookImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.NewGuid();
                Libro libro = await _libroService.GetLibroByIdService(model.BookId);
                BookImage bookImage = new()
                {
                    Libro = libro,
                    ImageId = imageId.ToString()+model.ImageFile.FileName
                };

                try
                {

                    _context.Add(bookImage);
                    await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Imagen agregada.");

                    var fileName = System.IO.Path.Combine(
                            _env.ContentRootPath, $"wwwroot/images/books",
                            $"{bookImage.ImageId}"
                        );

                    using (FileStream fs = new System.IO.FileStream(
                        fileName, System.IO.FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fs);
                    }

                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Libros
                                        .Include(l => l.BookImages)
                                        .FirstOrDefaultAsync(l => l.Codigo == model.BookId))
                    });
                }
                catch (Exception ex)
                {
                    _flashMessage.Danger(ex.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddImage", model) });
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BookImage bookImage = await _context.BookImages
                .Include(bi => bi.Libro)
                .FirstOrDefaultAsync(bi => bi.Id == id);

            if (bookImage == null)
            {
                return NotFound();
            }

            _context.BookImages.Remove(bookImage);
            await _context.SaveChangesAsync();

            var fileName = System.IO.Path.Combine(
                            _env.ContentRootPath, $"wwwroot\\images\\books\\", bookImage.ImageId.ToString()
                        );

            System.IO.File.Delete(fileName);

            _flashMessage.Info("Imagen borrada.");
            return RedirectToAction(nameof(Details), new { Id = bookImage.Libro.Codigo });
        }
    }
}
