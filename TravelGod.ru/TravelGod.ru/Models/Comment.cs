using System;

namespace TravelGod.ru.Models
{
    public class Comment
    {
        public virtual User User { get; set; }
        public virtual Trip Trip { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}