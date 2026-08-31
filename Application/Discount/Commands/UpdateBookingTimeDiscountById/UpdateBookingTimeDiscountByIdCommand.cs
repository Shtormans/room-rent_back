using Application.Abstractions;

namespace Application.Discount.Commands.UpdateBookingTimeDiscountById;

public record struct UpdateBookingTimeDiscountByIdCommand(Guid Id, TimeOnly From, TimeOnly To, decimal DiscountPercentage) : ICommand;