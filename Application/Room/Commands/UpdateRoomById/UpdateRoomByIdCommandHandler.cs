using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects.Room;

namespace Application.Room.Commands.UpdateRoomById;

public class UpdateRoomByIdCommandHandler : ICommandHandler<UpdateRoomByIdCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomByIdCommandHandler(IRoomRepository roomRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateRoomByIdCommand request, CancellationToken cancellationToken)
    {
        var roomNameResult = RoomName.Create(request.Name);
        if (roomNameResult.IsFailure)
        {
            return Result.Failure(roomNameResult.Error);
        }
        
        var roomCapacityResult = RoomCapacity.Create(request.Capacity);
        if (roomCapacityResult.IsFailure)
        {
            return Result.Failure(roomCapacityResult.Error);
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

        RoomSnapshot newRoom = new(request.Id)
        {
            Name = roomNameResult.Value,
            Capacity = roomCapacityResult.Value,
            BaseRentalRate = roomRentalRateResult.Value,
            Services = request.Services
        };
        
        bool updated = await _roomRepository.TryUpdate(newRoom, cancellationToken);
        if (!updated)
        {
            return Result.Failure(RoomErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        return Result.Success();
    }
}