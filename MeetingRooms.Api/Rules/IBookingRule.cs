namespace MeetingRooms.Api.Rules
{
    public interface IBookingRule
    {
        Task<BookingError?> ValidateAsync(BookingInformation bookingInformation);
    }
}