using Domain.Primitives;

namespace Domain.Entities;

public class BookingSnapshot(Guid id) : Entity(id)
{
    public required Guid RoomId { get; init; }
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
    public required decimal Price { get; init; }
    
    public required IReadOnlyList<Guid> Services { get; init; }
    
    public static BookingSnapshot Create(Guid roomId, DateTime start, DateTime end, decimal price, IReadOnlyList<Guid> services)
    {
        Guid id = Guid.NewGuid();
        
        return new BookingSnapshot(id)
        {
            RoomId = roomId,
            Start = start,
            End = end,
            Price = price,
            Services = services
        };
    }
}