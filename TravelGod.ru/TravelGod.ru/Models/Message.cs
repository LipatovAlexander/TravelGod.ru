using System;

namespace TravelGod.ru.Models
{
    public class Message
    {
        public virtual Chat Chat { get; set; }
        public virtual User User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}