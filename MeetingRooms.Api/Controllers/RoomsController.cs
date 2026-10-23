using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRooms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _rooms;

        public RoomsController(IRoomService room)
        {
            _rooms = room;
        }

        [HttpGet]
        public async Task<List<RoomDto>> GetAll([FromQuery] RoomFilter filter)
        {
            return await _rooms.GetAllAsync(filter);
        }
    }
}