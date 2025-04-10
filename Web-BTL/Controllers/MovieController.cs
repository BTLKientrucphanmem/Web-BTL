using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_BTL.Models;
using Web_BTL.Repository;

namespace Web_BTL.Controllers
{
    public class MovieController : Controller
    {
        private readonly DBXemPhimContext db;
        public MovieController(DBXemPhimContext context)
        {
            db = context;
        }
        public IActionResult Index(int movieId)
        {
            var movie = db.Medias.FirstOrDefault(m => m.MediaId == movieId);
            var customer = db.Customers.FirstOrDefault(c => c.UserEmail == HttpContext.Session.GetString("LogIn Session"));
            
            if (customer != null)
            {
                var watchListId = customer.WatchListId;

                bool isAlreadyInList = db.ListMedia
                    .Any(lm => lm.WatchListId == customer.WatchListId && lm.MediaId == movieId);
                if (!isAlreadyInList)
                {

                    var newListMedia = new ListMediaModel
                    {
                        WatchListId = customer.WatchListId,
                        MediaId = movieId,
                        IsWatched = false,
                        Favorite = false,
                        AddDate = DateTime.Now
                    };

                    db.ListMedia.Add(newListMedia);
                    db.SaveChanges();
                }
            }
            if (movie == null)
            {
                return NotFound(); // If the movie does not exist, return a 404 error
            }

            // Pass the movie object to the view
            return View(movie);
        }
        public IActionResult watching(int movieId)
        {
            var movie = db.Medias.FirstOrDefault(m => m.MediaId == movieId);
            var customer = db.Customers.FirstOrDefault(c => c.UserEmail == HttpContext.Session.GetString("LogIn Session"));
            if (customer != null)
            {
                var listMedia = db.ListMedia
                    .FirstOrDefault(lm => lm.WatchListId == customer.WatchListId && lm.MediaId == movieId);
                listMedia.IsWatched = true;
                db.SaveChanges();
            }
            if (movie == null)
            {
                return NotFound(); // If the movie does not exist, return a 404 error
            }

                // Pass the movie object to the view
            return View(movie);
        }
        [HttpPost]
        public IActionResult AddFavourite(int mid)
        {
            var customer = db.Customers.FirstOrDefault(c => c.UserEmail == HttpContext.Session.GetString("LogIn Session"));
            if (customer==null)
            {
                return PartialView("redirect-login");
            }
            var listMedia = db.ListMedia
                .FirstOrDefault(lm => lm.WatchListId == customer.WatchListId && lm.MediaId == mid);
            if (listMedia == null) return NotFound();
            listMedia.Favorite = true;
            db.SaveChanges();
            return PartialView("AddFavourite");
        }
    }
}
