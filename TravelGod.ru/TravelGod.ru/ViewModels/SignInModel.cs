using System.ComponentModel.DataAnnotations;
using TravelGod.ru.Infrastructure.Validation;

namespace TravelGod.ru.ViewModels
{
    public class SignInModel
    {
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Login { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}