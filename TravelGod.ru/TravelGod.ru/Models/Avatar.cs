namespace TravelGod.ru.Models
{
    public class Avatar
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public File File { get; set; }
        public int FileId { get; set; }
    }
}