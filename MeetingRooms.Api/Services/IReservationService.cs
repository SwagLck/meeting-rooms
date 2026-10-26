using MeetingRooms.Api.Dtos;

namespace MeetingRooms.Api.Services
{
    public interface IReservationService
    {
        Task<List<ReservationDto>> GetAllAsync(ReservationFilter filter);
        Task<ServiceResult<ReservationDto>> CreateAsync(ReservationRequest request);
        Task<ServiceResult<ReservationDto>> CancelAsync(int reservationId);
        Task<ServiceResult<ReservationDto>> EndEarlyAsync(int reservationId);
    }
}