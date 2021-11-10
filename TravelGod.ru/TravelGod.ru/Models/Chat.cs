using System;
using System.Collections.Generic;

namespace TravelGod.ru.Models
{
    public class Chat
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual User Initiator { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<Message> Messages { get; set; }
        public Status Status { get; set; }
    }
}