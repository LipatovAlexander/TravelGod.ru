using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TravelGod.ru.Models;

namespace TravelGod.ru.Pages
{
    public class SignUp : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(5, ErrorMessage = "Логин не должен быть короче 5 символов")]
        [MaxLength(10, ErrorMessage = "Логин не должен быть длиннее 10 символов")]
        [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "Логин должен состоять из букв и цифр")]
        public string Login { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Пароль не должен быть короче 5 символов")]
        [MaxLength(10, ErrorMessage = "Пароль не должен быть длиннее 10 символов")]
        [RegularExpression(@"^[A-Za-z0-9]{5,10}$", ErrorMessage = "Пароль должен состоять из букв и цифр")]
        public string Password1 { get; set; }

        [BindProperty]
        [Compare(nameof(Password1), ErrorMessage = "Пароли должны совпадать")]
        public string Password2 { get; set; }

        public SignUp(ApplicationContext context)
        {
            _context = context;
        }

        private readonly ApplicationContext _context;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage(nameof(SignIn));
        }
    }
}