using Application.Abstractions;
using Domain.Entities;

namespace Application.Discount.Queries.GetAllBookingTimeDiscounts;

public record struct GetAllBookingTimeDiscountsQuery : IQuery<List<BookingTimeDiscountSnapshot>>;