using MeetingRooms.Api.Options;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Api.Rules
{
    public class FutureStart : IBookingRule
    {
        private readonly TimeProvider _timeProvider;

        public FutureStart(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public Task<BookingError?> ValidateAsync(BookingInformation bookingInformation)
        {
            var now = _timeProvider.GetLocalNow();
            if (bookingInformation.Start <= now)
            {
                return Task.FromResult<BookingError?>(new BookingError(
                    "StartInPast",
                    "The start time of the booking cannot be in the past"));
            }

            if(bookingInformation.End <= bookingInformation.Start)
            {
                return Task.FromResult<BookingError?>(new BookingError(
                    "InvalidTimeRange", "The end time must be after the start time"
                ));
            }

            return Task.FromResult<BookingError?>(null);
        }
    }
}