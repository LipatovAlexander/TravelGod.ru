using System;

namespace TravelGod.ru.Models
{
    public class Message
    {
        public Chat Chat { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
    }
}