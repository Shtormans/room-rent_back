using Application.Abstractions;

namespace Application.Booking.Commands.DeleteBooking;

public record struct DeleteBookingByIdCommand(Guid Id) : ICommand;