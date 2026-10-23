using MeetingRooms.Api.Data;
using MeetingRooms.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Api.Services
{
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _db;

        public RoomService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<RoomDto>> GetAllAsync(RoomFilter filter)
        {
            IQueryable<Models.Room> query = _db.Rooms;

            if(filter.MinCapacity.HasValue)
            {
                query = query.Where(r => r.Capacity >= filter.MinCapacity.Value);
            }

            if(filter.Floor.HasValue)
            {
                query = query.Where(r => r.Floor == filter.Floor.Value);
            }

            if(filter.EquipmentIds is { Count: > 0 })
            {
                var ids = filter.EquipmentIds.Distinct().ToList();
                var requiredCount = ids.Count;

                query = query.Where(r => 
                r.Equipment.Count(e => ids.Contains(e.Id)) == requiredCount);
            }

            return await query
                .OrderBy(r => r.Name)
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Capacity = r.Capacity,
                    Floor = r.Floor,
                    IsAvailable = r.IsAvailable,
                    Equipment = r.Equipment
                    .Select(e => new EquipmentDto
                    {
                        Id = e.Id,
                        Name = e.Name
                    })
                    .ToList()
                })
                .ToListAsync();
        }

        public async Task<List<EquipmentDto>> GetEquipmentAsync()
        {
            return await _db.Equipment
                .OrderBy(e => e.Name)
                .Select(e => new EquipmentDto
                {
                    Id = e.Id,
                    Name = e.Name
                })
                .ToListAsync();
        }
    }
}