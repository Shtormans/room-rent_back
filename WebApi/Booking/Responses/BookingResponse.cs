namespace WebApi.Booking.Responses;

public class BookingResponse
{
    public required Guid Id { get; init; }
    
    public required Guid RoomId { get; init; }
    public required DateTime Start { get; init; }
    public required DateTime End { get; init; }
    public required decimal Price { get; init; }
    
    public required IReadOnlyList<Guid> Services { get; init; }
}