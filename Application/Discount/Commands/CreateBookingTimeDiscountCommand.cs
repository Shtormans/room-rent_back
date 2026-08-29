using Application.Abstractions;

namespace Application.Discount.Commands;

public record struct CreateBookingTimeDiscountCommand(TimeOnly From, TimeOnly To, decimal DiscountPercentage) : ICommand<Guid>;