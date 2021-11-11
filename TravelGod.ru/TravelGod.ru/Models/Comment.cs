﻿using System;

namespace TravelGod.ru.Models
{
    public class Comment
    {
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public Status Status { get; set; }
        public string Text { get; set; }
        public Trip Trip { get; set; }
        public User User { get; set; }
    }
}