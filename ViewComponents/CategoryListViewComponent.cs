using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _22DH112015_TranPhiLong.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = new List<string> { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
            return View(categories);
        }
    }
}