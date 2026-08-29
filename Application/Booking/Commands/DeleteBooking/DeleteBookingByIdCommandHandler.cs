using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Booking.Commands.DeleteBooking;

public class DeleteBookingByIdCommandHandler : ICommandHandler<DeleteBookingByIdCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookingByIdCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBookingByIdCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await _bookingRepository.TryDelete(request.Id, cancellationToken);
        
        if (!deleted)
        {
            return Result.Failure(BookingErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}