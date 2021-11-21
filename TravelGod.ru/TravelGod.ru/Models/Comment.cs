using System;
using System.ComponentModel.DataAnnotations;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.Models
{
    public class Comment
    {
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(100, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        [RegularExpression(@"^[A-Za-zА-Яа-я \.,\-!?""':\(\)\*]*$", ErrorMessage = ValidationMessages.RegularExpressionMessage)]
        public string Text { get; set; }
        public Trip Trip { get; set; }
        public User User { get; set; }
    }
}