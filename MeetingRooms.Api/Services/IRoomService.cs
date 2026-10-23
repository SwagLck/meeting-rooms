using MeetingRooms.Api.Dtos;

namespace MeetingRooms.Api.Services
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllAsync(RoomFilter filter);
        Task<List<EquipmentDto>> GetEquipmentAsync();
    }
}