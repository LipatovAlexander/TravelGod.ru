using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
        }
    }
}