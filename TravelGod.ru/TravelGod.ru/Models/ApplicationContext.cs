using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TravelGod.ru.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationContext (DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            var splitStringConverter = new ValueConverter<List<string>, string>(
                v => string.Join(";", v),
                v => v.Split(new[] { ';' }).ToList());
            builder
                .Entity<Trip>()
                .Property(nameof(Trip.Route))
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
                .HasMany(u => u.JoinedTrips)
                .WithOne(t => t.Initiator);

            builder
                .Entity<User>()
                .HasMany(u => u.OwnedTrips)
                .WithMany(t => t.Users)
                .UsingEntity(j => j.ToTable("usertrip"));
        }
    }
}