using Infrastructure.Entities;

namespace Domain.Entities;

public class RoomService
{
    public Guid RoomId { get; init; }
    public Room Room { get; set; } = null!;

    public Guid ServiceId { get; init; }
    public Service Service { get; set; } = null!;
}