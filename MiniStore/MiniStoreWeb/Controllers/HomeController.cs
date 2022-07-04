using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniStoreWeb.common;
using MiniStoreWeb.Data;
using MiniStoreWeb.Data.Entities;
using MiniStoreWeb.Models;
using System.Diagnostics;

namespace MiniStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
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


            IQueryable<Libro> query = _dbContext.Libros;

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
            //User user = await _userHelper.GetUserAsync(User.Identity.Name);
            //if (user != null)
            //{
            //    model.Quantity = await _context.TemporalSales
            //        .Where(ts => ts.User.Id == user.Id)
            //        .SumAsync(ts => ts.Quantity);
            //}

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
    }
}