using Microsoft.AspNetCore.Mvc;

namespace Web_BTL.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}
