using System;

namespace TravelGod.ru.Models
{
    public class Rating
    {
        public virtual Trip Trip { get; set; }
        public virtual User User { get; set; }
        public string Text { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}