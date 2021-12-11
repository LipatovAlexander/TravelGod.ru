namespace TravelGod.ru.Models
{
    public class File : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}