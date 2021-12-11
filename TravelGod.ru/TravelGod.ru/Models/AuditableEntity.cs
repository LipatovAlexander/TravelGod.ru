using System;

namespace TravelGod.ru.Models
{
    public class AuditableEntity
    {
        public User CreatedBy { get; set; }
        public int? CreatedById { get; set; }
        public User ModifiedBy { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}