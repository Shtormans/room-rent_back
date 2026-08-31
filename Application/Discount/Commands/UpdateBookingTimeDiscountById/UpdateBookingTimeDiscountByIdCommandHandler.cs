using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Discount.Commands.UpdateBookingTimeDiscountById;

public class UpdateBookingTimeDiscountByIdCommandHandler : ICommandHandler<UpdateBookingTimeDiscountByIdCommand>
{
    private readonly IBookingTimeDiscountRepository _bookingTimeDiscountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookingTimeDiscountByIdCommandHandler(IBookingTimeDiscountRepository bookingTimeDiscountRepository, IUnitOfWork unitOfWork)
    {
        _bookingTimeDiscountRepository = bookingTimeDiscountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateBookingTimeDiscountByIdCommand request, CancellationToken cancellationToken)
    {
        if (request.To < request.From)
        {
            return Result.Failure(DiscountErrors.Helpers.InvalidEndTime);
        }

        if (request.DiscountPercentage is <= 0 or > 1)
        {
            return Result.Failure(DiscountErrors.Helpers.InvalidDiscountPercentage);
        }

        BookingTimeDiscountSnapshot updatedDiscount = new(request.Id)
        {
            From = request.From,
            To = request.To,
            DiscountPercentage = request.DiscountPercentage
        };
        
        bool updated = await _bookingTimeDiscountRepository.TryUpdate(updatedDiscount, cancellationToken);
        if (!updated)
        {
            return Result.Failure(DiscountErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }
}