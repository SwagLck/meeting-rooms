namespace MeetingRooms.Api.Options
{
    public class BookingRulesOptions
    {
        public const string SectionName = "BookingRules";
        public int MaxDurationDays { get; set; }
        public int MaxDaysAhead { get; set; }
    }
}