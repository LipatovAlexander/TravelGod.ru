namespace TravelGod.ru.Infrastructure
{
    public static class RegularExpressions
    {
        public const string Text = @"^[A-Za-zА-Яа-яЁё \d\.,\-!?""':\(\)\*]*$";
        public const string Word = @"^[A-Za-zА-Яа-яЁё]*$";
        public const string LatinLettersAndDigits = @"^[A-Za-z0-9_]*$";
        public const string Dates = @"^\d\d\.\d\d\.\d\d\d\d - \d\d\.\d\d\.\d\d\d\d$";
    }
}