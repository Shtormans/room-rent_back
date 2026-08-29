using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Discount.Commands;

public class CreateBookingTimeDiscountCommandHandler : ICommandHandler<CreateBookingTimeDiscountCommand, Guid>
{
    private readonly IBookingTimeDiscountRepository _bookingTimeDiscountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingTimeDiscountCommandHandler(IBookingTimeDiscountRepository bookingTimeDiscountRepository, IUnitOfWork unitOfWork)
    {
        _bookingTimeDiscountRepository = bookingTimeDiscountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateBookingTimeDiscountCommand request, CancellationToken cancellationToken)
    {
        if (request.To < request.From)
        {
            return Result.Failure<Guid>(DiscountErrors.Helpers.InvalidEndTime);
        }

        if (request.DiscountPercentage <= 0)
        {
            return Result.Failure<Guid>(DiscountErrors.Helpers.InvalidDiscountPercentage);
        }

        var dto = BookingTimeDiscountDto.Create(request.From, request.To, request.DiscountPercentage);
        
        _bookingTimeDiscountRepository.Add(dto);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return dto.Id;
    }
}