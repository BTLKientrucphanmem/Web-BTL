using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web_BTL.Models;
using Web_BTL.Repository;

namespace Web_BTL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBXemPhimContext db;
        public HomeController(DBXemPhimContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var cartoons = db.Medias
                .Include(m => m.Genres)
                .Where(m => m.Genres.Any(g => g.Type == "Cartoon"))
                .ToList();
            ViewBag.Cartoon = cartoons;

            var movies = db.Medias
                .Include(m => m.Genres)
                .Where(m => m.Genres.Any(g => g.Type == "Movie"))
                .ToList();
            ViewBag.Movie = movies;

            var series = db.Medias
                .Include(m => m.Genres)
                .Where(m => m.Genres.Any(g => g.Type == "Series"))
                .ToList();
            ViewBag.Series = series;

            return View(db.Medias.ToList());
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

        public IActionResult GenreFilter(int id)
        {
            var movies = db.Medias
                .Include(m => m.Genres)
                .Where(m => m.Genres.Any(g => g.GenreId == id))
                .ToList();

            return View(movies);
        }

        public IActionResult CartoonMovieFilter()
        {
            var movies = db.Medias
                .Include(m => m.Genres)
                .Where(m => m.Genres.Any(g => g.Type == "Cartoon"))
                .ToList();
            ViewBag.Cartoon = movies;
            return View();
        }
    }
}
