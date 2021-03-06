using System;

namespace TravelGod.ru.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public bool RememberMe { get; set; }
    }
}