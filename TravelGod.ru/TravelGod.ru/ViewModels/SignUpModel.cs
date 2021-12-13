using System.ComponentModel.DataAnnotations;
using TravelGod.ru.Infrastructure.Validation;

namespace TravelGod.ru.ViewModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MinLength(5, ErrorMessage = ValidationMessages.MinLengthMessage)]
        [MaxLength(10, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(RegularExpressions.LatinLettersAndDigits,
            ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Login { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MinLength(5, ErrorMessage = ValidationMessages.MinLengthMessage)]
        [MaxLength(10, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(RegularExpressions.LatinLettersAndDigits,
            ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Password1 { get; set; }

        [Compare(nameof(Password1), ErrorMessage = "Пароли должны совпадать")]
        public string Password2 { get; set; }
    }
}