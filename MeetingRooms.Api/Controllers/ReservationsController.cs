using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRooms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservations;

        public ReservationsController(IReservationService reservations)
        {
            _reservations = reservations;
        }

        [HttpGet]
        public async Task<List<ReservationDto>> GetAll([FromQuery] ReservationFilter filter)
        {
            return await _reservations.GetAllAsync(filter);
        }
    }
}