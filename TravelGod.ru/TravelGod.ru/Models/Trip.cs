using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelGod.ru.Models
{
    public class Trip
    {
        public Chat Chat { get; set; }
        public List<Comment> Comments { get; set; } = new();

        [Required(ErrorMessage = "Добавьте описание")]
        [RegularExpression(@"^[A-Za-zА-Яа-я\d ,\.!?""']$", ErrorMessage = "Описание содержит недопустимые символы")]
        [MaxLength(200, ErrorMessage = "Описание должно не должно быть длиннее 200 символов")]
        public string Description { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int Id { get; set; }

        [Required]
        public User Initiator { get; set; }
        public List<Rating> Ratings { get; set; } = new();

        [Required]
        public List<string> Route { get; set; } = new();

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [MaxLength(30, ErrorMessage = "Название не может быть длиннее 20 символов")]
        [RegularExpression(@"^[A-Za-zА-Яа-я\d ,\.!?""']$", ErrorMessage = "Название содержит недопустимые символы")]
        public string Title { get; set; }
        public List<User> Users { get; set; } = new();

        public int UsersCount { get; set; }
    }
}