using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Booking.Queries.GetAllBookings;

public class GetAllBookingsQueryHandler : IQueryHandler<GetAllBookingQuery, List<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<List<BookingDto>>> Handle(GetAllBookingQuery request, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetAll(cancellationToken);
    }
}