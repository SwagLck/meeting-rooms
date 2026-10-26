using MeetingRooms.Api.Rules;

namespace MeetingRooms.Api.Services
{
    public class ServiceResult<T>
    {
        public T? Value { get; set; }
        public BookingError? Error { get; private set; }
        public bool IsSuccess => Error == null;

        public static ServiceResult<T> Ok(T value) => new() { Value = value };
        public static ServiceResult<T> Fail(BookingError error) => new() { Error = error };
    }
}