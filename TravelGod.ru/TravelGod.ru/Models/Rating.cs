using System.ComponentModel.DataAnnotations;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.Models
{
    public class Rating : AuditableEntity
    {
        public int Id { get; set; }

        [Required] public Point Point { get; set; }

        public Status Status { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [RegularExpression(RegularExpressions.Text, ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        [MaxLength(150, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Text { get; set; }

        public Trip Trip { get; set; }
    }

    public enum Point
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
}