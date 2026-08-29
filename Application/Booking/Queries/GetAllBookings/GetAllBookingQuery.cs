using Application.Abstractions;
using Domain.Entities;

namespace Application.Booking.Queries.GetAllBookings;

public record struct GetAllBookingQuery : IQuery<List<BookingDto>>;