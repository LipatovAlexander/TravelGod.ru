using System;
using System.Collections.Generic;

namespace TravelGod.ru.Models
{
    public class Chat
    {
        public DateTime CreationDate { get; set; }
        public int Id { get; set; }
        public User Initiator { get; set; }
        public List<Message> Messages { get; set; } = new();
        public string Name { get; set; }
        public Status Status { get; set; }
        public List<User> Users { get; set; } = new();
    }
}