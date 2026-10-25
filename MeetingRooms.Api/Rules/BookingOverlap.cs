using MeetingRooms.Api.Data;
using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Rules
{
    public class BookingOverlap : IBookingRule
    {
        private readonly AppDbContext _dbContext;

        public BookingOverlap(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookingError?> ValidateAsync(BookingInformation bookingInformation)
        {
            var hasOverlap = await _dbContext.Reservations.AnyAsync(r =>
                r.RoomId == bookingInformation.Room.Id && 
                (bookingInformation.ExcludeReservationId == null || r.Id != bookingInformation.ExcludeReservationId) &&
                r.Status == ReservationStatus.Cancelled &&
                r.Start < bookingInformation.End &&
                bookingInformation.Start < r.End);

            if (hasOverlap)
            {
                return new BookingError(
                    "BookingOverlap",
                    "The requested booking time overlaps with an existing booking.");
            }

            return null;
        }
    }
}