using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using TravelGod.ru.Infrastructure.Validation;

namespace TravelGod.ru.Models
{
    public class Trip : AuditableEntity
    {
        public Chat Chat { get; set; }
        public List<Comment> Comments { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [RegularExpression(RegularExpressions.Text,
            ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        [MaxLength(300, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public int Id { get; set; }

        public List<Rating> Ratings { get; set; }
        public double AverageRating { get; set; }

        [NotMapped] public List<string> Route => RouteRaw?.Split(';').ToList();

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required] public Status Status { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(30, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(RegularExpressions.Text, ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Title { get; set; }

        public List<User> Users { get; set; }

        [Required] public int UsersCount { get; set; }

        [Required] public TripType Type { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(200, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(RegularExpressions.Text, ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string RouteRaw { get; set; }

        [NotMapped]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [RegularExpression(RegularExpressions.Dates, ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string DatesRaw
        {
            get => StartDate.ToString("dd.MM.yyyy") + " - " + EndDate.ToString("dd.MM.yyyy");
            set
            {
                value = value.Trim();
                if (!DateTime.TryParseExact(value[..10], "dd.MM.yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var startDate) ||
                    !DateTime.TryParseExact(value[13..23], "dd.MM.yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var endDate))
                {
                    return;
                }

                StartDate = startDate;
                EndDate = endDate;
            }
        }
    }

    public enum TripType
    {
        [Display(Name = "Поездка на природу")] NatureTrip,
        [Display(Name = "Городская поездка")] CityTrip,
        [Display(Name = "Всё вместе")] CommonTrip
    }
}