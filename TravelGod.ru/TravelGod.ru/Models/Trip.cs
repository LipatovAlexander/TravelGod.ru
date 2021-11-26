﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.Models
{
    public class Trip
    {
        public Chat Chat { get; set; }
        public List<Comment> Comments { get; set; } = new();

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [RegularExpression(@"^[A-Za-zА-Яа-я\d ,\.!?""\-']*$",
            ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        [MaxLength(300, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public int Id { get; set; }

        [Required] public int InitiatorId { get; set; }

        public User Initiator { get; set; }
        public List<Rating> Ratings { get; set; } = new();

        [NotMapped] public List<string> Route => RouteRaw?.Split(';').ToList();

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required] public Status Status { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(30, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(@"^[A-Za-zА-Яа-я\d ,\.!?""']*$", ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Title { get; set; }

        public List<User> Users { get; set; } = new();

        [Required] public int UsersCount { get; set; }

        [Required] public TripType Type { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(200, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(@"^[A-Za-zА-Яа-я ,-;]*$", ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string RouteRaw { get; set; }

        [NotMapped]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [RegularExpression(@"^\d\d\.\d\d\.\d\d\d\d - \d\d\.\d\d\.\d\d\d\d$", ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string DatesRaw
        {
            get => StartDate.ToString("dd.MM.yyyy") + " - " + EndDate.ToString("dd.MM.yyyy");
            set
            {
                value = value.Trim();
                if (!DateTime.TryParse(value[..10], out var startDate) ||
                    !DateTime.TryParse(value[13..23], out var endDate))
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