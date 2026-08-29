using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Booking.Queries.SearchAvailableRooms;

public class SearchAvailableRoomsQueryHandler : IQueryHandler<SearchAvailableRoomsQuery, List<Guid>>
{
    private readonly IBookingRepository _bookingRepository;

    public SearchAvailableRoomsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Result<List<Guid>>> Handle(SearchAvailableRoomsQuery request, CancellationToken cancellationToken)
    {
        DateTime currentDate = DateTime.UtcNow;
        if (request.Start < currentDate)
        {
            return Result.Failure<List<Guid>>(BookingErrors.Helpers.InvalidStartDate);
        }

        if (request.Start >= request.End)
        {
            return Result.Failure<List<Guid>>(BookingErrors.Helpers.InvalidEndDate);
        }

        if (request.Capacity <= 0)
        {
            return Result.Failure<List<Guid>>(BookingErrors.Helpers.InvalidCapacity);
        }

        List<Guid> rooms = await _bookingRepository.GetAvailableRooms(request.Start, request.End, request.Capacity, cancellationToken);

        return rooms;
    }
}