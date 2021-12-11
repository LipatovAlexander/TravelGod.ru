using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace TravelGod.ru.Models
{
    public class ApplicationContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationContext(DbContextOptions<ApplicationContext> options,
                                  IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Avatar> Avatars { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Avatar>()
                .Navigation(a => a.File)
                .AutoInclude();

            builder
                .Entity<Chat>()
                .HasOne(c => c.ModifiedBy)
                .WithMany();

            builder
                .Entity<Trip>()
                .HasOne(c => c.ModifiedBy)
                .WithMany();

            builder
                .Entity<File>()
                .HasOne(f => f.CreatedBy)
                .WithMany(u => u.Files)
                .HasForeignKey(f => f.CreatedById);

            builder
                .Entity<User>()
                .HasMany(u => u.OwnedChats)
                .WithOne(c => c.CreatedBy);

            builder
                .Entity<User>()
                .HasMany(u => u.JoinedChats)
                .WithMany(c => c.Users)
                .UsingEntity(j => j.ToTable("userchat"));

            builder
                .Entity<User>()
                .HasMany(u => u.OwnedTrips)
                .WithOne(t => t.CreatedBy);

            builder
                .Entity<User>()
                .HasMany(u => u.JoinedTrips)
                .WithMany(t => t.Users)
                .UsingEntity(j => j.ToTable("usertrip"));

            var defaultAvatarFile = new File
            {
                Id = 1,
                Name = "default-avatar.png",
                Path = "CustomFiles/Avatars/default-avatar.png"
            };

            builder
                .Entity<File>()
                .HasData(defaultAvatarFile);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                          .Entries()
                          .Where(e => e.Entity is AuditableEntity &&
                                      e.State is EntityState.Added or EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((AuditableEntity) entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((AuditableEntity) entityEntry.Entity).CreatedBy =
                        _httpContextAccessor?.HttpContext?.Items["User"] as User;
                }
                else
                {
                    Entry((AuditableEntity) entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((AuditableEntity) entityEntry.Entity).Reference(p => p.CreatedBy).IsModified = false;
                }

                ((AuditableEntity) entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
                ((AuditableEntity) entityEntry.Entity).ModifiedBy =
                    _httpContextAccessor?.HttpContext?.Items["User"] as User;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}