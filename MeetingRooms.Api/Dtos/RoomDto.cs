namespace MeetingRooms.Api.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public int Floor { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<EquipmentDto> Equipment { get; set; } = new();
    }
}