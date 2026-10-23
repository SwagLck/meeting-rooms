using MeetingRooms.Api.Dtos;

namespace MeetingRooms.Api.Services
{
    public interface IReservationService
    {
        Task<List<ReservationDto>> GetAllAsync(ReservationFilter filter);
    }
}