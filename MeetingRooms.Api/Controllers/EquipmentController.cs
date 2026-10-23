using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRooms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IRoomService _room;

        public EquipmentController(IRoomService room)
        {
            _room = room;
        }

        [HttpGet]
        public async Task<List<EquipmentDto>> GetAll()
        {
            return await _room.GetEquipmentAsync();
        }
    }
}