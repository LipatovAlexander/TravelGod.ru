using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TravelGod.ru.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var splitStringConverter = new ValueConverter<string, string>(
                v => string.Join(";", v
                    .Split(new[] {' ', ';', '-', ','}, StringSplitOptions.RemoveEmptyEntries)),
                v => v);
            builder
                .Entity<Trip>()
                .Property(nameof(Trip.RouteRaw))
                .HasConversion(splitStringConverter);

            builder
                .Entity<User>()
                .HasMany(u => u.OwnedChats)
                .WithOne(c => c.Initiator);

            builder
                .Entity<User>()
                .HasMany(u => u.JoinedChats)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("userchat"));

            builder
                .Entity<User>()
                .HasMany(u => u.OwnedTrips)
                .WithOne(t => t.Initiator);

            builder
                .Entity<User>()
                .HasMany(u => u.JoinedTrips)
                .WithMany(t => t.Users)
                .UsingEntity(j => j.ToTable("usertrip"));

            var defaultAvatar = new File
            {
                Id = 1,
                Name = "default-avatar.png",
                Path = "CustomFiles/Avatars/default-avatar.png"
            };
            builder
                .Entity<File>()
                .HasData(defaultAvatar);
        }
    }
}