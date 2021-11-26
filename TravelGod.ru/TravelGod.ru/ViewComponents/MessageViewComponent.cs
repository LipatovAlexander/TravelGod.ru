using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class MessageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Message message)
        {
            return View(message);
        }
    }
}