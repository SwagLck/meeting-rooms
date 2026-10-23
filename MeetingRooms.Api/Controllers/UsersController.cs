using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRooms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _users;

        public UsersController(IUserService users)
        {
            _users = users;
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAll()
        {
            return await _users.GetAllAsync();
        }
    }
}