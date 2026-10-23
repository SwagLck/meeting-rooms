using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Data;
using MeetingRooms.Api.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("MeetingRooms"));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Seed the database with some initial data
    db.Rooms.AddRange(
        new Room { Name = "Room A", Capacity = 10, Floor = 1, IsAvailable = true },
        new Room { Name = "Room B", Capacity = 20, Floor = 2, IsAvailable = false },
        new Room { Name = "Room C", Capacity = 15, Floor = 1, IsAvailable = true }
    );

    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
