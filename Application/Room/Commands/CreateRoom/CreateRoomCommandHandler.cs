using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects.Room;
using MediatR;

namespace Application.Room.Commands.CreateRoom;

public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Guid>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomNameResult = RoomName.Create(request.Name);
        if (roomNameResult.IsFailure)
        {
            return Result.Failure<Guid>(roomNameResult.Error);
        }
        
        var roomCapacityResult = RoomCapacity.Create(request.Capacity);
        if (roomCapacityResult.IsFailure)
        {
            return Result.Failure<Guid>(roomCapacityResult.Error);
        }
        
        var roomRentalRateResult = RoomRentalRate.Create(request.BaseRentalRate);
        if (roomRentalRateResult.IsFailure)
        {
            return Result.Failure<Guid>(roomRentalRateResult.Error);
        }

        foreach (Guid serviceId in request.Services)
        {
            bool exists = await _serviceRepository.Exists(serviceId, cancellationToken);

            if (!exists)
            {
                return Result.Failure<Guid>(ServiceErrors.Helpers.NotFound(serviceId));
            }
        }
        
        var room = RoomDto.Create(roomNameResult.Value, roomCapacityResult.Value, roomRentalRateResult.Value, request.Services);
        
        _roomRepository.Add(room);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return room.Id;
    }
}