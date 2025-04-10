using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Web_BTL.Models;
using Web_BTL.Repository;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Web_BTL.Models;

namespace Web_BTL.Controllers
{
    public class GenreController : Controller
    {
        private DBXemPhimContext db;
        public GenreController(DBXemPhimContext _db)
        {
            db = _db;
        }
        public IActionResult Index(int? actorId, int? genreId, string quality, string duration)
        {
            var medias = db.Medias.AsQueryable();

            if (actorId.HasValue)
            {
                medias = medias.Where(m => m.Actors.Any(a => a.Actor.ActorID == actorId.Value));
            }

            if (genreId.HasValue)
            {
                medias = medias.Where(m => m.Genres.Any(g => g.GenreId == genreId.Value));
            }

            if (!string.IsNullOrEmpty(quality))
            {
                medias = medias.Where(m => m.MediaQuality == quality);
            }

            if (!string.IsNullOrEmpty(duration))
            {
                switch (duration)
                {
                    case "short":
                        medias = medias.Where(m => m.MediaDuration.HasValue && m.MediaDuration.Value <= TimeSpan.FromMinutes(60));
                        break;
                    case "medium":
                        medias = medias.Where(m => m.MediaDuration.HasValue && m.MediaDuration.Value > TimeSpan.FromMinutes(60) && m.MediaDuration.Value <= TimeSpan.FromMinutes(120));
                        break;
                    case "long":
                        medias = medias.Where(m => m.MediaDuration.HasValue && m.MediaDuration.Value > TimeSpan.FromMinutes(120));
                        break;
                }
            }

            // Tạo dữ liệu cho DropdownList để hiển thị trong View
            ViewBag.AllActors = db.Actors.Select(a => new SelectListItem
            {
                Text = a.ActorName,
                Value = a.ActorID.ToString()
            }).ToList();

            ViewBag.AllGenres = db.Genres.Select(g => new SelectListItem
            {
                Text = g.Type,
                Value = g.GenreId.ToString()
            }).ToList();

            ViewBag.AllQualities = db.Medias.Select(m => m.MediaQuality).Distinct().ToList();

            return View(medias.ToList());
        }



        public IActionResult AllMedias(int? pageindex, string searchTerm)
        {
            // Khởi tạo IQueryable chứa danh sách MediaModel từ cơ sở dữ liệu
            var medias = db.Medias.AsQueryable();

            // Nếu có từ khóa tìm kiếm, lọc kết quả dựa trên tên phim
            if (!string.IsNullOrEmpty(searchTerm))
            {
                medias = medias.Where(m => m.MediaName.Contains(searchTerm));
            }

            // Xác định số trang hiện tại
            var page = pageindex ?? 1;

            // Định nghĩa số lượng items trên mỗi trang
            int pageSize = 8;  // Bạn có thể thay đổi giá trị PageSize tùy theo yêu cầu

            // Tính tổng số trang dựa trên số lượng items và pageSize
            int totalItems = medias.Count();
            int pageNumbers = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Gán tổng số trang vào ViewBag để hiển thị trong View
            ViewBag.PageNumbers = pageNumbers;
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = searchTerm;

            // Lấy các item cho trang hiện tại, sử dụng Skip và Take để phân trang
            var result = medias
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Trả về View cùng với danh sách các MediaModel của trang hiện tại
            return View(result);
        }


        public IActionResult MoviesFilter(int? mid)
        {
            int PageSize = 8;
            var medias = (IQueryable<MediaModel>)db.Medias;

            if (mid != null)
            {
                medias = (IQueryable<MediaModel>)db.Medias
                    .Where(m => m.Genres.Any(g => g.GenreId == mid));

                ViewBag.mid = mid;
            }

            var result = medias.Take(PageSize).ToList();

            return PartialView("_MediaPartial", result);
        }



        public IActionResult FilteredMedias(int genreId)
        {
            var medias = db.Medias
                                 .Where(m => m.Genres.Any(g => g.GenreId == genreId))
                                 .ToList();
            return PartialView("Layouts/_MediaPartial", medias);
        }


    }
}