using TravelGod.ru.Models;

namespace TravelGod.ru.ViewModels
{
    public class UserFilter
    {
        public const int PageSize = 10;
        public int PageNumber { get; set; } = 1;

        public string Name { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
    }
}