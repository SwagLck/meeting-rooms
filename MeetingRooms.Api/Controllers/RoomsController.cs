using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Data;
using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _db;

    public RoomsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<Room>> Getall()
    {
        return await _db.Rooms.ToListAsync();
    }
}