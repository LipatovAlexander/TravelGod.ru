using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace TravelGod.ru.Models
{
    public class Trip
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Route { get; set; }
        public virtual User Initiator { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual Chat Chat { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Rating> Ratings { get; set; }
        public Status Status { get; set; }
    }
}