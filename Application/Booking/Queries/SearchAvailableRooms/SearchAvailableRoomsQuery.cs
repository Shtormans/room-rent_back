using Application.Abstractions;

namespace Application.Booking.Queries.SearchAvailableRooms;

public record struct SearchAvailableRoomsQuery(DateTime Start, DateTime End, int Capacity) : IQuery<List<Guid>>;