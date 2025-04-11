using Microsoft.AspNetCore.Mvc;
using Web_BTL.DataAccessLayer;

namespace Web_BTL.ViewComponents {
    public class PackageViewComponent : ViewComponent
    {
        private readonly DBXemPhimContext _dataContext;
        public PackageViewComponent(DBXemPhimContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return View("Default");
        }
    }
}
