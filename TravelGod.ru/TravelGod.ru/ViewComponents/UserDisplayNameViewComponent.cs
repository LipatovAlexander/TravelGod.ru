using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class UserDisplayNameViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(User user)
        {
            return View(user);
        }
    }
}