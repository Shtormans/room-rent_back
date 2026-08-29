using System.ComponentModel.DataAnnotations;

namespace WebApi.Booking.Requests;

public class BookRoomRequest
{
    [Required] public required Guid RoomId { get; init; }
    [Required] public required DateTime Start { get; init; }
    [Required, Range(1, 24)] public required int DurationHours { get; init; }
    [Required] public required IReadOnlyList<Guid> Services { get; init; }
}