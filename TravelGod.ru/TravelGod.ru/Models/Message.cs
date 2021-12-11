using System.ComponentModel.DataAnnotations;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.Models
{
    public class Message : AuditableEntity
    {
        public Chat Chat { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(100, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(RegularExpressions.Text, ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Text { get; set; }
    }
}