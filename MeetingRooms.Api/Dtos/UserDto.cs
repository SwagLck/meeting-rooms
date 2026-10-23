namespace MeetingRooms.Api.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int ActiveReservationsCount { get; set; } = 0;
    }
}