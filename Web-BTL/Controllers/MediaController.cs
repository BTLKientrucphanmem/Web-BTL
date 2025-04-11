using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_BTL.DataAccessLayer;
using Web_BTL.DataAccessLayer.Models;

namespace Web_BTL.BusinessLogicLayer.Controllers {
    public class MediaController : Controller
    {
        private DBXemPhimContext db;
        private int PageSize = 16;
        public IActionResult Index(int? mid, int page = 1)
        {
            var media = (IQueryable<MediaModel>)db.Medias
                .Include(m => m.Genres);
            if (mid != null)
            {
                media = (IQueryable<MediaModel>)db.Medias
                    .Where(m => m.Genres.Any(g => g.GenreId == mid ));
            }

            int totalMedia = media.Count();
            int pageNumbers = (int)Math.Ceiling(totalMedia / (float)PageSize);

            ViewBag.PageNumbers = pageNumbers;
            ViewBag.CurrentPage = page;

            var result = media
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            return View(result);
        }


        public IActionResult MoviesFilter(int? mid, string? keyword, int? pageindex)
        {
            var medias = (IQueryable<MediaModel>)db.Medias;
            var page = (int)(pageindex == null || pageindex <= 0 ? 1 : pageindex);

            if (mid != null)
            {
                medias = (IQueryable<MediaModel>)db.Medias
                    .Where(m => m.Genres.Any(g => g.GenreId == mid));

                ViewBag.mid = mid;
            }

            if(keyword != null)
            {
                medias = (IQueryable<MediaModel>)db.Medias
                    .Where(m => m.MediaName.Contains(keyword));
                ViewBag.keyword = keyword;
            }

            int pageNumbers = (int)Math.Ceiling(medias.Count() / (float)PageSize);
            ViewBag.PageNumbers = pageNumbers;
            var result = medias
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return PartialView("_MediaPartial", result);
        }
    }
}
