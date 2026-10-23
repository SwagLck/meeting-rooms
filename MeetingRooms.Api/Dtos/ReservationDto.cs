namespace MeetingRooms.Api.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = "";
        public int OrganizerId { get; set; }
        public string OrganizerName { get; set; } = "";
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int NumberOfParticipants { get; set; }
        public string Status { get; set; } = "";
    }
}