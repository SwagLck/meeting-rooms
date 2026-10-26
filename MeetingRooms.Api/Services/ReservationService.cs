using System.Reflection.Metadata.Ecma335;
using MeetingRooms.Api.Data;
using MeetingRooms.Api.Dtos;
using MeetingRooms.Api.Models;
using MeetingRooms.Api.Rules;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Api.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _db;
        private readonly TimeProvider _time;
        private readonly IEnumerable<IBookingRule> _bookingRules;

        public ReservationService(AppDbContext db, TimeProvider time, IEnumerable<IBookingRule> bookingRules)
        {
            _db = db;
            _time = time;
            _bookingRules = bookingRules;
        }

        public async Task<List<ReservationDto>> GetAllAsync(ReservationFilter filter)
        {
            IQueryable<Reservation> query = _db.Reservations;

            if(filter.RoomId.HasValue)
            {
                query = query.Where(r => r.RoomId == filter.RoomId.Value);
            }

            if(filter.OrganizerId.HasValue)
            {
                query = query.Where(r => r.OrganizerId == filter.OrganizerId.Value);
            }

            if(filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status.Value);
            }

            if(filter.From.HasValue)
            {
                query = query.Where(r => r.Start >= filter.From.Value);
            }

            if(filter.To.HasValue)
            {
                query = query.Where(r => r.End <= filter.To.Value);
            }

            return await query
                .OrderByDescending(r => r.Start)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    RoomId = r.RoomId,
                    RoomName = r.Room.Name,
                    OrganizerId = r.OrganizerId,
                    OrganizerName = r.Organizer.FirstName + " " + r.Organizer.LastName,
                    Title = r.Title,
                    Start = r.Start,
                    End = r.End,
                    NumberOfParticipants = r.NumberOfParticipants,
                    Status = r.Status.ToString()
                })
                .ToListAsync();
        }

        public static ReservationDto ToDto(Reservation r) => new()
        {
            Id = r.Id,
            RoomId = r.RoomId,
            RoomName = r.Room.Name,
            OrganizerId = r.OrganizerId,
            OrganizerName = r.Organizer.FirstName + " " + r.Organizer.LastName,
            Title = r.Title,
            Start = r.Start,
            End = r.End,
            NumberOfParticipants = r.NumberOfParticipants,
            Status = r.Status.ToString()
        };

        public async Task<ServiceResult<ReservationDto>> CancelAsync(int id)
        {
            var reservation = await _db.Reservations
                .Include(r => r.Room)
                .Include(r => r.Organizer)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(reservation is null)
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("ReservationNotFound", $"Reservation {id} does not exist")
                );
            }

            if(reservation.Status != ReservationStatus.Booked)
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("InvalidStatus", "Only booked reservations can be cancelled")
                );
            }

            reservation.Status = ReservationStatus.Cancelled;
            await _db.SaveChangesAsync();

            return ServiceResult<ReservationDto>.Ok(ToDto(reservation));
        }

        public async Task<ServiceResult<ReservationDto>> EndEarlyAsync(int id)
        {
            var now = _time.GetLocalNow().DateTime;

            var reservation = await _db.Reservations
                .Include(r => r.Room)
                .Include(r => r.Organizer)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(reservation is null)
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("ReservationNotFound", "Reservation {id} does not exist")
                );
            }

            var inProgress = reservation.Status == ReservationStatus.Booked
                                    && reservation.Start <= now
                                    && now < reservation.End;

            if (!inProgress)
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("MeetingNotInProgress", "Only a meeting that is happening now cna be ended early")
                );
            }

            reservation.End = now;
            reservation.Status = ReservationStatus.Completed;
            await _db.SaveChangesAsync();

            return ServiceResult<ReservationDto>.Ok(ToDto(reservation));
        }

        public async Task<ServiceResult<ReservationDto>> CreateAsync(ReservationRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Title))
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("InvalidTitle", "The reservation title cannot be empty."));
            }

            var room = await _db.Rooms.FindAsync(request.RoomId);
            if (room == null)
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("RoomNotFound", $"Room {request.RoomId} does not exist."));
            }

            var organizer = await _db.Users.FindAsync(request.OrganizerId);
            if (organizer == null)
            {
                return ServiceResult<ReservationDto>.Fail(
                    new BookingError("OrganizerNotFound", $"User {request.OrganizerId} does not exist."));
            }


            var bookingInformation = new BookingInformation
            {
                Room = room,
                OrganizerId = request.OrganizerId,
                Start = request.Start,
                End = request.End,
                NumberOfParticipants = request.NumberOfParticipants,
                Now = _time.GetLocalNow().DateTime

            };

            foreach (var rule in _bookingRules)
            {
                var error = await rule.ValidateAsync(bookingInformation);
                if (error != null)
                {
                    return ServiceResult<ReservationDto>.Fail(error);
                }
            }

            var reservation = new Reservation
            {
                RoomId = request.RoomId,
                OrganizerId = request.OrganizerId,
                Title = request.Title,
                Start = request.Start,
                End = request.End,
                NumberOfParticipants = request.NumberOfParticipants,
                Status = ReservationStatus.Booked
            };

            _db.Reservations.Add(reservation);
            await _db.SaveChangesAsync();

            var reservationDto = new ReservationDto
            {
                Id = reservation.Id,
                RoomId = reservation.RoomId,
                RoomName = reservation.Room.Name,
                OrganizerId = reservation.OrganizerId,
                OrganizerName = reservation.Organizer.FirstName + " " + reservation.Organizer.LastName,
                Title = reservation.Title,
                Start = reservation.Start,
                End = reservation.End,
                NumberOfParticipants = reservation.NumberOfParticipants,
                Status = reservation.Status.ToString()
            };

            return ServiceResult<ReservationDto>.Ok(reservationDto);
        }
        
    }
}