using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Discount.Queries.GetAllBookingTimeDiscounts;

public class GetAllBookingTimeDiscountsQueryHandler : IQueryHandler<GetAllBookingTimeDiscountsQuery, List<BookingTimeDiscountSnapshot>>
{
    private readonly IBookingTimeDiscountRepository _bookingTimeDiscountRepository;

    public GetAllBookingTimeDiscountsQueryHandler(IBookingTimeDiscountRepository bookingTimeDiscountRepository)
    {
        _bookingTimeDiscountRepository = bookingTimeDiscountRepository;
    }

    public async Task<Result<List<BookingTimeDiscountSnapshot>>> Handle(GetAllBookingTimeDiscountsQuery request, CancellationToken cancellationToken)
    {
        return await _bookingTimeDiscountRepository.GetAll(cancellationToken);
    }
}