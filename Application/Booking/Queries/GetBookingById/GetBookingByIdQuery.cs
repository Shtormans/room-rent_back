using Application.Abstractions;
using Domain.Entities;

namespace Application.Booking.Queries.GetBookingById;

public record struct GetBookingByIdQuery(Guid Id) : IQuery<BookingDto>;