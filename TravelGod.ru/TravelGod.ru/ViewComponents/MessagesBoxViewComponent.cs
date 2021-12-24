using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class MessagesBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Chat chat)
        {
            return View(chat);
        }
    }
}