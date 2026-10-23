using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Equipment> Equipment => Set<Equipment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasMany(r => r.Equipment)
                .WithMany();
        }
    }
}