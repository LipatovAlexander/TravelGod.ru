using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.Models
{
    public class Rating
    {
        public DateTime Date { get; set; }
        public int Id { get; set; }
        [Required]
        public Point Point { get; set; }
        public Status Status { get; set; }
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [RegularExpression(RegularExpressions.Text, ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        [MaxLength(400, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Text { get; set; }
        public Trip Trip { get; set; }
        public User User { get; set; }
    }

    public enum Point
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
}