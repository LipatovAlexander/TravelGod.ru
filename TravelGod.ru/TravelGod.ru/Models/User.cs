using System;
using System.Collections.Generic;

namespace TravelGod.ru.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
        public virtual List<Chat> Chats { get; set; }
        public virtual List<Trip> Trips { get; set; }
    }

    public enum Role
    {
        User,
        Moderator,
        Administrator
    }
}