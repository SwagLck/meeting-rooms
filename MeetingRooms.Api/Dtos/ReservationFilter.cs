using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Dtos
{
    public class ReservationFilter
    {
        public int? RoomId { get; set; }
        public int? OrganizerId { get; set; }
        public ReservationStatus? Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}