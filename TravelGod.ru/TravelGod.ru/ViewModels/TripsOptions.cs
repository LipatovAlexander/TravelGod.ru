using System.ComponentModel.DataAnnotations;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.Models
{
    public class TripsOptions
    {
        public const int PageSize = 10;

        [RegularExpression(@"^[A-Za-zА-Яа-я0-9\., '""]*$", ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Title { get; set; }

        [RegularExpression(@"^[A-Za-zА-Яа-я0-9\., '""]*$", ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Route { get; set; }

        public bool Archive { get; set; }

        [RegularExpression(@"\d\d.\d\d.\d\d\d\d - \d\d.\d\d.\d\d\d\d",
            ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Dates { get; set; }

        public int PageNumber { get; set; } = 1;
    }
}