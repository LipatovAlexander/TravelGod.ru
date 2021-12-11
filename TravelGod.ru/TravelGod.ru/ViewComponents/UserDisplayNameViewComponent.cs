using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class UserDisplayNameViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            return View(user);
        }
    }
}