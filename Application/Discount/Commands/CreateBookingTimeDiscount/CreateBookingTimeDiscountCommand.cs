using Application.Abstractions;

namespace Application.Discount.Commands.CreateBookingTimeDiscount;

public record struct CreateBookingTimeDiscountCommand(TimeOnly From, TimeOnly To, decimal DiscountPercentage) : ICommand<Guid>;