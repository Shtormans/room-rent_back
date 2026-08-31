using Application.Abstractions;

namespace Application.Discount.Commands.DeleteBookingTimeDiscountById;

public record struct DeleteBookingTimeDiscountByIdCommand(Guid Id) : ICommand;