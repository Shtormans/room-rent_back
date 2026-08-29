using System.ComponentModel.DataAnnotations;
using Domain.ValueObjects.Room;

namespace WebApi.Room.Requests;

public class UpdateRoomResponse
{
    [Required, StringLength(RoomName.MaxLength, MinimumLength = 1)] public required string Name { get; init; }
    [Required, Range(1, int.MaxValue)] public required int Capacity { get; init; }
    [Required, Range(0, double.MaxValue)] public required decimal BaseRentalRate { get; init; }
    [Required] public required IReadOnlyList<Guid> Services { get; init; }
}