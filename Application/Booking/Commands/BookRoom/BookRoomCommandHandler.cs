using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.Utils;

namespace Application.Booking.Commands.BookRoom;

public class BookRoomCommandHandler : ICommandHandler<BookRoomCommand, BookRoomCommandResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingTimeDiscountRepository _bookingTimeDiscountRepository;

    public BookRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork, 
        IBookingRepository bookingRepository, IBookingTimeDiscountRepository bookingTimeDiscountRepository)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _bookingRepository = bookingRepository;
        _bookingTimeDiscountRepository = bookingTimeDiscountRepository;
    }

    public async Task<Result<BookRoomCommandResponse>> Handle(BookRoomCommand request, CancellationToken cancellationToken)
    {
        DateTime currentDate = DateTime.UtcNow;
        if (request.Start < currentDate)
        {
            return Result.Failure<BookRoomCommandResponse>(BookingErrors.Helpers.InvalidStartDate);
        }

        DateTime end = DateTimeUtils.AddDuration(request.Start, request.DurationHours);
        if (request.Start >= end)
        {
            return Result.Failure<BookRoomCommandResponse>(BookingErrors.Helpers.InvalidEndDate);
        }

        RoomSnapshot? room = await _roomRepository.GetById(request.RoomId, cancellationToken);

        if (room is null)
        {
            return Result.Failure<BookRoomCommandResponse>(RoomErrors.Helpers.NotFound(request.RoomId));
        }

        bool hasInvalidServices = request.Services.Any(service => !room.Services.Contains(service));
        if (hasInvalidServices)
        {
            return Result.Failure<BookRoomCommandResponse>(BookingErrors.Helpers.InvalidServices);
        }
        
        List<Guid> rooms = await _bookingRepository.GetAvailableRooms(request.Start, end, 0, cancellationToken);
        if (!rooms.Contains(request.RoomId))
        {
            return Result.Failure<BookRoomCommandResponse>(BookingErrors.Helpers.AlreadyBooked(request.RoomId));
        }
        
        TimeOnly bookTime = TimeOnly.FromDateTime(request.Start);
        BookingTimeDiscountSnapshot? discountDto = await _bookingTimeDiscountRepository.GetDiscount(bookTime, cancellationToken);
        decimal discount = discountDto?.DiscountPercentage ?? 0;
        
        decimal price = await _bookingRepository.CalculateBaseBookingPrice(request.RoomId, request.DurationHours, request.Services, cancellationToken);
        price -= price * discount;
        
        var booking = BookingSnapshot.Create(request.RoomId, request.Start, end, price, request.Services);
        
        _bookingRepository.Add(booking);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new BookRoomCommandResponse(booking.Id, price);
    }
}