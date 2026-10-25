using MeetingRooms.Api.Options;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Api.Rules
{
    public class Capacity : IBookingRule
    {
        public Task<BookingError?> ValidateAsync(BookingInformation bookingInformation)
        {
            if (bookingInformation.NumberOfParticipants > bookingInformation.Room.Capacity)
            {
                return Task.FromResult<BookingError?>(new BookingError(
                    "CapacityExceeded",
                    $"The number of participants ({bookingInformation.NumberOfParticipants}) exceeds the room capacity ({bookingInformation.Room.Capacity})."));
            }

            return Task.FromResult<BookingError?>(null);
        }
    }
}