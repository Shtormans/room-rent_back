using Domain.Primitives;

namespace Domain.Entities;

public class BookingDto(Guid id) : Entity(id)
{
    public required Guid RoomId { get; init; }
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
    public required decimal Price { get; init; }
    
    public required IReadOnlyList<Guid> Services { get; init; }
    
    public static BookingDto Create(Guid roomId, DateTime start, DateTime end, decimal price, IReadOnlyList<Guid> services)
    {
        Guid id = Guid.NewGuid();
        
        return new BookingDto(id)
        {
            RoomId = roomId,
            Start = start,
            End = end,
            Price = price,
            Services = services
        };
    }
}