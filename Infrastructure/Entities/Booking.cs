namespace Infrastructure.Entities;

public sealed class Booking
{
    public Guid Id { get; set; }
    
    public Guid RoomId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public decimal Price { get; set; }
    
    public List<BookingService> BookingServices { get; set; }
}