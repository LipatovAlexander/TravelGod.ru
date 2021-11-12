using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class IndexModel : MyPageModel
    {
        public IndexModel(ApplicationContext context) : base(context)
        {
        }

        public void OnGet()
        {
        }
    }
}