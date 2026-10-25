namespace MeetingRooms.Api.Rules
{
    public class Available : IBookingRule
    {
        Task<BookingError?> IBookingRule.ValidateAsync(BookingInformation bookingInformation)
        {
            if (!bookingInformation.Room.IsAvailable)
            {
                return Task.FromResult<BookingError?>(new BookingError("RoomNotAvailable", "The room is not available for booking."));
            }

            return Task.FromResult<BookingError?>(null);
        }
    }
}