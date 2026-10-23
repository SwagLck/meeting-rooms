using MeetingRooms.Api.Data;
using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Api.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly TimeProvider _time;


        public UserService(AppDbContext db, TimeProvider time)
        {
            _db = db;
            _time = time;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var now = _time.GetLocalNow().DateTime;
            

            return await _db.Users
                .OrderBy(u => u.LastName)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    ActiveReservationsCount = _db.Reservations.Count( r=>
                        r.OrganizerId == u.Id &&
                        r.Status == ReservationStatus.Booked &&
                        r.End > now)
                })
                .ToListAsync();
        }
    }
}