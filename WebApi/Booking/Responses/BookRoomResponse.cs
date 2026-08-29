namespace WebApi.Booking.Responses;

public class BookRoomResponse
{
    public required Guid BookingId { get; init; }
    public required decimal Price { get; init; }
}