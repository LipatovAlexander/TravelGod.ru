using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelGod.ru.Infrastructure.Validation;

namespace TravelGod.ru.Models
{
    public enum Role
    {
        User,
        Administrator
    }

    public class User
    {
        [RegularExpression(RegularExpressions.Text, ErrorMessage = "Описание содержит недопустимые символы")]
        [MaxLength(100, ErrorMessage = "Описание не должно быть длиннее 100 символов")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        public List<Chat> JoinedChats { get; set; }

        [EmailAddress] public string Email { get; set; }

        [RegularExpression(RegularExpressions.Word, ErrorMessage = "Имя содержит недопустимые символы")]
        [MaxLength(20, ErrorMessage = "Имя не должно быть длиннее 20 символов")]
        public string FirstName { get; set; }

        [RegularExpression(@"^\d*$")] public int Id { get; set; }

        [RegularExpression(RegularExpressions.Word, ErrorMessage = "Фамилия содержит недопустимые символы")]
        [MaxLength(20, ErrorMessage = "Фамилия не должна быть длиннее 20 символов")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(RegularExpressions.LatinLettersAndDigits,
            ErrorMessage = "Логин содержит недопустимые символы")]
        [MaxLength(10, ErrorMessage = "Логин не должен быть длиннее 10 символов")]
        public string Login { get; set; }

        [Required] public string PasswordHash { get; set; }

        [Required] public string PasswordSalt { get; set; }

        [RegularExpression(RegularExpressions.Word, ErrorMessage = "Отчество содержит недопустимые символы")]
        [MaxLength(20, ErrorMessage = "Отчество не должно быть длиннее 20 символов")]
        public string Patronymic { get; set; }

        public Avatar Avatar { get; set; }

        [Required] public Role Role { get; set; }

        [Required] public Status Status { get; set; }

        public List<Trip> JoinedTrips { get; set; }
        public List<Chat> OwnedChats { get; set; }
        public List<Trip> OwnedTrips { get; set; }

        [NotMapped]
        public string DisplayName => !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
            ? FirstName + " " + LastName
            : Login;

        public int OwnedTripsCount { get; set; }
        public int JoinedTripsCount { get; set; }
        public List<File> Files { get; set; }
    }
}