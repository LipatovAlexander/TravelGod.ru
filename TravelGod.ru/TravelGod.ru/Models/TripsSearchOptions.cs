using System;
using System.ComponentModel.DataAnnotations;

namespace TravelGod.ru.Models
{
    public class TripsSearchOptions
    {
        private string _datesRaw;

        private string _routeRaw;

        [RegularExpression(@"^[A-Za-zА-Яа-я0-9\., '""]*$", ErrorMessage = "Название содержит недопустимые символы")]
        public string Title { get; set; }

        [RegularExpression(@"^[A-Za-zА-Яа-я0-9\., '""]*$", ErrorMessage = "Маршрут содержит недопустимые символы")]
        public string RouteRaw
        {
            get => _routeRaw;
            set
            {
                _routeRaw = value;
                Route = string.IsNullOrEmpty(value)
                    ? Array.Empty<string>()
                    : value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public bool Archive { get; set; }

        [RegularExpression(@"\d\d.\d\d.\d\d\d\d - \d\d.\d\d.\d\d\d\d", ErrorMessage = "Некорректный диапазон дат")]
        public string DatesRaw
        {
            get => _datesRaw;
            set
            {
                _datesRaw = value;
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var startDateRaw = value[..10];
                var endDateRaw = value[13..23];
                StartDate = DateTime.Parse(startDateRaw);
                EndDate = DateTime.Parse(endDateRaw);
            }
        }

        public bool HasTitle => !string.IsNullOrEmpty(Title);

        public string[] Route { get; set; }

        public bool HasRoute => Route?.Length > 0;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool HasDates => !string.IsNullOrEmpty(DatesRaw);
    }
}