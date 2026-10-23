namespace MeetingRooms.Api.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public int Floor { get; set; }
        public bool IsAvailable { get; set; } = true;

        public List<Equipment> Equipment { get; set; } = new();
    }
}