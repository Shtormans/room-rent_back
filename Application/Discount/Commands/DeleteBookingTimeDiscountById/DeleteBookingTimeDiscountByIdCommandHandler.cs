using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Discount.Commands.DeleteBookingTimeDiscountById;

public class DeleteBookingTimeDiscountByIdCommandHandler : ICommandHandler<DeleteBookingTimeDiscountByIdCommand>
{
    private readonly IBookingTimeDiscountRepository _bookingTimeDiscountRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteBookingTimeDiscountByIdCommandHandler(IBookingTimeDiscountRepository bookingTimeDiscountRepository, IUnitOfWork unitOfWork)
    {
        _bookingTimeDiscountRepository = bookingTimeDiscountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBookingTimeDiscountByIdCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await _bookingTimeDiscountRepository.TryDelete(request.Id, cancellationToken);
        
        if (!deleted)
        {
            return Result.Failure(BookingErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }
}