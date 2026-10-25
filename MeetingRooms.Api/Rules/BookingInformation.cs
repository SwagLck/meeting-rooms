using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Rules
{
    public class BookingInformation
    {
        public Room Room { get; set; } = null!;
        public int OrganizerId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime Now { get; set; }

        public int? ExcludeReservationId { get; set; }
    }
}