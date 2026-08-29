using Domain.Entities;

namespace Infrastructure.Entities;

public sealed class Room
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BaseRentalRate { get; set; }
    
    public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}