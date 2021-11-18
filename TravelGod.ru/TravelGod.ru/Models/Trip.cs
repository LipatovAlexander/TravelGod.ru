using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TravelGod.ru.Models
{
    public class Trip
    {
        public Chat Chat { get; set; }
        public List<Comment> Comments { get; set; } = new();

        [Required(ErrorMessage = "Добавьте описание")]
        [RegularExpression(@"^[A-Za-zА-Яа-я\d ,\.!?""']*$", ErrorMessage = "Описание содержит недопустимые символы")]
        [MaxLength(300, ErrorMessage = "Описание не должно быть длиннее 300 символов")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ввведите дату окончания поездки")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public int Id { get; set; }

        [Required] public int InitiatorId { get; set; }

        public User Initiator { get; set; }
        public List<Rating> Ratings { get; set; } = new();

        [NotMapped]
        public List<string> Route => RouteRaw?.Split(';').ToList();

        [Required(ErrorMessage = "Ввведите дату начала поездки")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required] public Status Status { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [MaxLength(30, ErrorMessage = "Название не может быть длиннее 20 символов")]
        [RegularExpression(@"^[A-Za-zА-Яа-я\d ,\.!?""']*$", ErrorMessage = "Название содержит недопустимые символы")]
        public string Title { get; set; }

        public List<User> Users { get; set; } = new();

        [Required] public int UsersCount { get; set; }

        [Required] public TripType Type { get; set; }

        public bool CreateChat { get; set; }

        [Required(ErrorMessage = "Введите маршрут")]
        [RegularExpression(@"^[A-Za-zА-Яа-я ,-;]*$", ErrorMessage = "Маршрут содержит недопустимые символы")]
        public string RouteRaw { get; set; }
    }

    public enum TripType
    {
        [Display(Name = "Поездка на природу")]
        NatureTrip,
        [Display(Name = "Городская поездка")]
        CityTrip,
        [Display(Name = "Всё вместе")]
        CommonTrip
    }
}