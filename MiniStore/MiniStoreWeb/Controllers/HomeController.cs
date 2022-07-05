using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreWeb.common;
using MiniStoreWeb.Data;
using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Helpers;
using MiniStoreWeb.Models;
using System.Diagnostics;

namespace MiniStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserHelper _userHelper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext,
            IUserHelper userHelper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "PriceDesc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            IQueryable<Libro> query = _dbContext.Libros
                .Include(l => l.BookImages);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(l => (l.Titulo.ToLower().Contains(searchString.ToLower())) && l.Stock > 0);
            }
            else
            {
                query = query.Where(l => l.Stock > 0);
            }

            switch (sortOrder)
            {
                case "NameDesc":
                    query = query.OrderByDescending(l => l.Titulo);
                    break;
                case "Price":
                    query = query.OrderBy(l => l.Precio);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(l => l.Precio);
                    break;
                default:
                    query = query.OrderBy(p => p.Titulo);
                    break;
            }

            int pageSize = 8;

            HomeViewModel model = new()
            {
                Libros = await PaginatedList<Libro>.CreateAsync(query, pageNumber ?? 1, pageSize),
            };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _dbContext.TemporalSales
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Libro libro = await _dbContext.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = new()
            {
                Libro = libro,
                Quantity = 1,
                User = user
            };

            _dbContext.TemporalSales.Add(temporalSale);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Libro libro = await _dbContext.Libros
                .Include(p => p.BookImages)
                .FirstOrDefaultAsync(p => p.Codigo == id);
            if (libro == null)
            {
                return NotFound();
            }

            AddBookToCartViewModel model = new()
            {
                Description = libro.Descripcion,
                Id = libro.Codigo,
                Name = libro.Titulo,
                Price = libro.Precio,
                BookImages = libro.BookImages,
                Quantity = 1,
                Stock = libro.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddBookToCartViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Libro libro = await _dbContext.Libros.FindAsync(model.Id);
            if (libro == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = new()
            {
                Libro = libro,
                Quantity = model.Quantity,
                Remarks = model.Remarks,
                User = user
            };

            _dbContext.TemporalSales.Add(temporalSale);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<TemporalSale> temporalSales = await _dbContext.TemporalSales
                .Include(ts => ts.Libro)
                .ThenInclude(p => p.BookImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            ShowCartViewModel model = new()
            {
                User = user,
                TemporalSales = temporalSales
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.TemporalSales = await _dbContext.TemporalSales
                .Include(ts => ts.Libro)
                .ThenInclude(p => p.BookImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            //Response response = await _ordersHelper.ProcessOrderAsync(model);
            //if (response.IsSuccess)
            //{
            //    return RedirectToAction(nameof(OrderSuccess));
            //}

            //ModelState.AddModelError(string.Empty, response.Message);

            return View(model);
        }

        public async Task<IActionResult> DecreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _dbContext.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            if (temporalSale.Quantity > 1)
            {
                temporalSale.Quantity--;
                _dbContext.TemporalSales.Update(temporalSale);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> IncreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSales = await _dbContext.TemporalSales.FindAsync(id);
            if (temporalSales == null)
            {
                return NotFound();
            }

            temporalSales.Quantity++;
            _dbContext.TemporalSales.Update(temporalSales);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _dbContext.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            _dbContext.TemporalSales.Remove(temporalSale);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _dbContext.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            EditTemporalSaleViewModel model = new()
            {
                Id = temporalSale.Id,
                Quantity = temporalSale.Quantity,
                Remarks = temporalSale.Remarks,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTemporalSaleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TemporalSale temporalSale = await _dbContext.TemporalSales.FindAsync(id);
                    temporalSale.Quantity = model.Quantity;
                    temporalSale.Remarks = model.Remarks;
                    _dbContext.Update(temporalSale);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }

                return RedirectToAction(nameof(ShowCart));
            }

            return View(model);
        }
    }
}