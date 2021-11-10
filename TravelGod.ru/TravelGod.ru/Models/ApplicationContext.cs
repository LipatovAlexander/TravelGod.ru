using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TravelGod.ru.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(
                v => string.Join(";", v),
                v => v.Split(new[] { ';' }));
            builder
                .Entity<Trip>()
                .Property(nameof(Trip.Route))
                .HasConversion(splitStringConverter);
        }
    }
}