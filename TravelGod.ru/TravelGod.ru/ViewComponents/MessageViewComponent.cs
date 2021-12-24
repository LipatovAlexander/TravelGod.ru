using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class MessageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Message message)
        {
            return View(message);
        }
    }
}