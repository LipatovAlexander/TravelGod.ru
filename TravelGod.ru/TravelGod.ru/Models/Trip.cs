using System;
using System.Collections.Generic;

namespace TravelGod.ru.Models
{
    public class Trip
    {
        public Chat Chat { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public int Id { get; set; }
        public User Initiator { get; set; }
        public List<Rating> Ratings { get; set; } = new();
        public List<string> Route { get; set; } = new();
        public DateTime StartDate { get; set; }
        public Status Status { get; set; }
        public string Title { get; set; }
        public List<User> Users { get; set; } = new();
    }
}