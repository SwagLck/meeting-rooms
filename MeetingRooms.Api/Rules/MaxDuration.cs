using Microsoft.Extensions.Options;
using MeetingRooms.Api.Options;

namespace MeetingRooms.Api.Rules
{
    public class MaxDuration : IBookingRule
    {
        private readonly BookingRulesOptions _options;

        public MaxDuration(IOptions<BookingRulesOptions> options)
        {
            _options = options.Value;
        }

        public Task<BookingError?> ValidateAsync(BookingInformation bookingInformation)
        {
            var duration = bookingInformation.End - bookingInformation.Start;
            if (duration.TotalDays > _options.MaxDurationDays)
            {
                return Task.FromResult<BookingError?>(new BookingError(
                    "MaxDurationExceeded",
                    $"The maximum duration for a booking is {_options.MaxDurationDays} days."));
            }

            return Task.FromResult<BookingError?>(null);
        }
    }
}