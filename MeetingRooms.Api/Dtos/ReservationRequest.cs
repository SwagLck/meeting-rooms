namespace MeetingRooms.Api.Dtos
{
    public class ReservationRequest
    {
        public int RoomId { get; set; }
        public int OrganizerId { get; set; }
        public string Title { get; set; } ="";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}