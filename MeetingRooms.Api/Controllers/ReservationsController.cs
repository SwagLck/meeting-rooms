using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Services;
using MeetingRooms.Api.Models;
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

        [HttpPost]
        public async Task<IActionResult> Create(ReservationRequest request)
        {
            var result = await _reservations.CreateAsync(request);
            if (!result.IsSuccess)
            {
                return Conflict(result.Error);
            }

            return Created($"api/reservations/{result.Value!.Id}", result.Value);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int reservationId)
        {
            var result = await _reservations.CancelAsync(reservationId);
            return ToActionResult(result);
        }

        [HttpPost("{id}/end")]
        public async Task<IActionResult> EndEarly(int reservationId)
        {
            var result = await _reservations.EndEarlyAsync(reservationId);
            return ToActionResult(result);
        }

        private IActionResult ToActionResult(ServiceResult<ReservationDto> result)
        {
            if(result.IsSuccess) return Ok(result.Value);

            return result.Error!.Code == "ReservationNotFound"
                    ? NotFound(result.Error)
                    : Conflict(result.Error);
        }
    }
}