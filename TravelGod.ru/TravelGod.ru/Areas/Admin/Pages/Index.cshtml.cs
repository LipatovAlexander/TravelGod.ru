using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Pages;
using TravelGod.ru.Services;

namespace TravelGod.ru.Areas.Admin.Pages
{
    [AdministratorPageFilter]
    public class Index : MyPageModel
    {
        public void OnGet()
        {

        }
    }
}