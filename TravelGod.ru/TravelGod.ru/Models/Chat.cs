using System.Collections.Generic;

namespace TravelGod.ru.Models
{
    public class Chat : AuditableEntity
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }
        public List<User> Users { get; set; }
        public bool IsGroupChat { get; set; }
    }
}