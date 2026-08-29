using Domain.Primitives;
using Domain.ValueObjects.Room;

namespace Domain.Entities;

public class RoomDto(Guid id) : Entity(id)
{
    public required RoomName Name { get; init; }
    public required RoomCapacity Capacity { get; init; }
    public required RoomRentalRate BaseRentalRate { get; init; }
    public required IReadOnlyList<Guid> Services { get; init; }

    public static RoomDto Create(RoomName name, RoomCapacity capacity, RoomRentalRate baseRentalRate, IReadOnlyList<Guid> services)
    {
        Guid id = Guid.NewGuid();
        
        return new RoomDto(id)
        {
            Name = name,
            Capacity = capacity,
            BaseRentalRate = baseRentalRate,
            Services = services
        };
    }
}