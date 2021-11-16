using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression(@"^[А-Яа-яA-Za-z\.,!?""' ]*$", ErrorMessage = "Описание содержит недопустимые символы")]
        [MaxLength(100, ErrorMessage = "Описание не должно быть длиннее 100 символов")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        public List<Chat> JoinedChats { get; set; }

        [EmailAddress] public string Email { get; set; }

        [RegularExpression(@"^[A-Za-zА-Яа-я]*$", ErrorMessage = "Имя содержит недопустимые символы")]
        [MaxLength(20, ErrorMessage = "Имя не должно быть длиннее 20 символов")]
        public string FirstName { get; set; }

        [RegularExpression(@"^\d*$")] public int Id { get; set; }

        [RegularExpression(@"^[A-Za-zА-Яа-я]*$", ErrorMessage = "Фамилия содержит недопустимые символы")]
        [MaxLength(20, ErrorMessage = "Фамилия не должна быть длиннее 20 символов")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9_]*$", ErrorMessage = "Логин содержит недопустимые символы")]
        [MaxLength(10, ErrorMessage = "Логин не должен быть длиннее 10 символов")]
        public string Login { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string PasswordSalt { get; set; }

        [RegularExpression(@"^[A-Za-zА-Яа-я]*$", ErrorMessage = "Отчество содержит недопустимые символы")]
        [MaxLength(20, ErrorMessage = "Отчество не должно быть длиннее 20 символов")]
        public string Patronymic { get; set; }

        public File Avatar { get; set; }

        [Required]
        public Role Role { get; set; }
        [Required]
        public Status Status { get; set; }
        public List<Trip> JoinedTrips { get; set; } = new();
        public List<Chat> OwnedChats { get; set; } = new();
        public List<Trip> OwnedTrips { get; set; } = new();
    }
}