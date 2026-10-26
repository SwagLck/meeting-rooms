using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Data;
using MeetingRooms.Api.Models;
using MeetingRooms.Api.Services;
using MeetingRooms.Api.Options;
using MeetingRooms.Api.Rules;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
//

// Add booking rules
builder.Services.Configure<BookingRulesOptions>(
    builder.Configuration.GetSection(BookingRulesOptions.SectionName));

// RULES LIST:
builder.Services.AddScoped<IBookingRule, MaxDuration>();
builder.Services.AddScoped<IBookingRule, FutureStart>();
builder.Services.AddScoped<IBookingRule, Capacity>();
builder.Services.AddScoped<IBookingRule, Available>();
builder.Services.AddScoped<IBookingRule, BookingIntoFuture>();
builder.Services.AddScoped<IBookingRule, BookingOverlap>();
builder.Services.AddScoped<IBookingRule, OrganizerOverlap>();
//

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod());
});

builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);


builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("MeetingRooms"));
var app = builder.Build();

app.UseCors("Frontend");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
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
