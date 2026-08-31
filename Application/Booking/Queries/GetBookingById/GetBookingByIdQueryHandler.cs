using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Booking.Queries.GetBookingById;

public class GetBookingByIdQueryHandler : IQueryHandler<GetBookingByIdQuery, BookingSnapshot>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<BookingSnapshot>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _bookingRepository.GetById(request.Id, cancellationToken);

        if (room is null)
        {
            return Result.Failure<BookingSnapshot>(BookingErrors.Helpers.NotFound(request.Id));
        }

        return room;
    }
}