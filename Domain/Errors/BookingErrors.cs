using Domain.Shared;

namespace Domain.Errors;

public static class BookingErrors
{
    public static class Codes
    {
        public const string InvalidServices = "Booking.InvalidServices";
        public const string InvalidStartDate = "Booking.InvalidStartDate";
        public const string InvalidEndDate = "Booking.InvalidEndDate";
        public const string InvalidCapacity = "Booking.InvalidCapacity";
        public const string NotFound = "Booking.NotFound";
        public const string AlreadyBooked = "Booking.AlreadyBooked";
    }

    public static class Helpers
    {
        public static readonly Error InvalidServices = new(Codes.InvalidServices, $"Booking was provided with invalid services.");
        public static readonly Error InvalidStartDate = new(Codes.InvalidStartDate, $"Start date was invalid.");
        public static readonly Error InvalidEndDate = new(Codes.InvalidEndDate, $"End date was invalid.");
        public static readonly Error InvalidCapacity = new(Codes.InvalidCapacity, $"Capacity must be greater than 0.");
        
        public static Error NotFound(Guid bookingId) => new(Codes.NotFound, $"Booking with id {bookingId} was not found.");
        public static Error AlreadyBooked(Guid roomId) => new(Codes.AlreadyBooked, $"Room with id {roomId} is already booked between this time.");
    }
}