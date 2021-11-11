using System;
using System.Collections.Generic;

namespace TravelGod.ru.Models
{
    public enum Role
    {
        User,
        Moderator,
        Administrator
    }

    public class User
    {
        public DateTime BirthDate { get; set; }
        public List<Chat> JoinedChats { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Patronymic { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
        public List<Trip> JoinedTrips { get; set; } = new();
        public List<Chat> OwnedChats { get; set; } = new();
        public List<Trip> OwnedTrips { get; set; } = new();
    }
}