using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class CommentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Comment comment)
        {
            return View(comment);
        }
    }
}