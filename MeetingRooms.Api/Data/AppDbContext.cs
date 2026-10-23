using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
}
