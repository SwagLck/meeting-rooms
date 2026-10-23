namespace MeetingRooms.Api.Dtos
{
    /// <summary>
    /// Represents a filter for searching rooms based on capacity, floor, and equipment.
    /// Filter can be checked and unchecked.
    /// </summary>
    public class RoomFilter
    {
        public int? MinCapacity { get; set; }
        public int? Floor { get; set; }
        public List<int>? EquipmentIds { get; set; }
    }
}