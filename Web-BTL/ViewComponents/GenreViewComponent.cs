using Microsoft.AspNetCore.Mvc;
using Web_BTL.DataAccessLayer;

namespace WebBTL.ViewComponents {
    public class GenreViewComponent : ViewComponent
    {
        private readonly DBXemPhimContext _context;

        public GenreViewComponent(DBXemPhimContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isAdmin = false)
        {
            var genres = _context.Genres.ToList();
            if (isAdmin) return View("AdminGenre", genres);
            return View("Default", genres);
        }
    }
}
