using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class MessagesBoxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Chat chat)
        {
            return View(chat);
        }
    }
}