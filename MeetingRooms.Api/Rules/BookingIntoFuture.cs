using MeetingRooms.Api.Options;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Api.Rules
{
    public class BookingIntoFuture : IBookingRule
    {
        private readonly TimeProvider _timeProvider;
        private readonly BookingRulesOptions _bookingRulesOptions;

        public BookingIntoFuture(TimeProvider timeProvider, IOptions<BookingRulesOptions> bookingRulesOptions)
        {
            _timeProvider = timeProvider;
            _bookingRulesOptions = bookingRulesOptions.Value;
        }

        public Task<BookingError?> ValidateAsync(BookingInformation bookingInformation)
        {
            var now = _timeProvider.GetLocalNow().DateTime;
            var maxDate = now.AddDays(_bookingRulesOptions.MaxDaysAhead);

            if (bookingInformation.Start > maxDate)
            {
                return Task.FromResult<BookingError?>(new BookingError(
                    "StartTooFarInFuture",
                    $"The start time of the booking cannot be more than {_bookingRulesOptions.MaxDaysAhead} days in the future."));
            }

            return Task.FromResult<BookingError?>(null);
        }
    }
}