using MeetingRooms.Api.Data;
using Microsoft.EntityFrameworkCore;
using MeetingRooms.Api.Models;

namespace MeetingRooms.Api.Rules
{
    public class OrganizerOverlap : IBookingRule
    {
        private readonly AppDbContext _dbContext;

        public OrganizerOverlap(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookingError?> ValidateAsync(BookingInformation bookingInformation)
        {
            var hasOverlap = await _dbContext.Reservations.AnyAsync(r =>
                r.OrganizerId == bookingInformation.OrganizerId && 
                (bookingInformation.ExcludeReservationId == null || r.Id != bookingInformation.ExcludeReservationId) &&
                r.Status != ReservationStatus.Cancelled &&
                r.Start < bookingInformation.End &&
                bookingInformation.Start < r.End);

            if (hasOverlap)
            {
                return new BookingError(
                    "OrganizerOverlap",
                    "The requested booking time overlaps with an existing booking.");
            }

            return null;
        }
    }
}