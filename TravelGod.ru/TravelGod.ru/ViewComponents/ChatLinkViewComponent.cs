using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;

namespace TravelGod.ru.ViewComponents
{
    public class ChatLinkViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Chat chat)
        {
            return View(chat);
        }
    }
}