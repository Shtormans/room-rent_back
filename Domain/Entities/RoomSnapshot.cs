using Domain.Primitives;
using Domain.ValueObjects.Room;

namespace Domain.Entities;

public class RoomSnapshot(Guid id) : Entity(id)
{
    public required RoomName Name { get; init; }
    public required RoomCapacity Capacity { get; init; }
    public required RoomRentalRate BaseRentalRate { get; init; }
    public required IReadOnlyList<Guid> Services { get; init; }

    public static RoomSnapshot Create(RoomName name, RoomCapacity capacity, RoomRentalRate baseRentalRate, IReadOnlyList<Guid> services)
    {
        Guid id = Guid.NewGuid();
        
        return new RoomSnapshot(id)
        {
            Name = name,
            Capacity = capacity,
            BaseRentalRate = baseRentalRate,
            Services = services
        };
    }
}