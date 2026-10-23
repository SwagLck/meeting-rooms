using MeetingRooms.Api.Dtos;

namespace MeetingRooms.Api.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
    }
}