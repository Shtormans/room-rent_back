using Application.Abstractions;

namespace Application.Booking.Commands.BookRoom;

public record struct BookRoomCommand(Guid RoomId, DateTime Start, int DurationHours, IReadOnlyList<Guid> Services) : ICommand<BookRoomCommandResponse>;