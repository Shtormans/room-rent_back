namespace WebApi.Room.Responses;

public class RoomResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required decimal Capacity { get; init; }
    public required decimal BaseRentalRate { get; init; }
    public required IReadOnlyList<Guid> Services { get; init; }
}