namespace MeetingRooms.Api.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public int OrganizerId { get; set; }
        public User Organizer { get; set; } = null!;
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumberOfParticipants { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Booked;
    }
}