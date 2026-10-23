using MeetingRooms.Api.Data;
using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Api.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _db;

        public ReservationService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ReservationDto>> GetAllAsync(ReservationFilter filter)
        {
            IQueryable<Reservation> query = _db.Reservations;

            if(filter.RoomId.HasValue)
            {
                query = query.Where(r => r.RoomId == filter.RoomId.Value);
            }

            if(filter.OrganizerId.HasValue)
            {
                query = query.Where(r => r.OrganizerId == filter.OrganizerId.Value);
            }

            if(filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status.Value);
            }

            if(filter.From.HasValue)
            {
                query = query.Where(r => r.Start >= filter.From.Value);
            }

            if(filter.To.HasValue)
            {
                query = query.Where(r => r.End <= filter.To.Value);
            }

            return await query
                .OrderByDescending(r => r.Start)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    RoomId = r.RoomId,
                    RoomName = r.Room.Name,
                    OrganizerId = r.OrganizerId,
                    OrganizerName = r.Organizer.FirstName + " " + r.Organizer.LastName,
                    Title = r.Title,
                    Start = r.Start,
                    End = r.End,
                    NumberOfParticipants = r.NumberOfParticipants,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }
        
    }
}