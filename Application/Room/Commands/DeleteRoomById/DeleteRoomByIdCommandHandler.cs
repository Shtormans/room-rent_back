using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Room.Commands.DeleteRoomById;

public class DeleteRoomByIdCommandHandler : ICommandHandler<DeleteRoomByIdCommand>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteRoomByIdCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteRoomByIdCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await _roomRepository.TryDelete(request.Id, cancellationToken);
        
        if (!deleted)
        {
            return Result.Failure(RoomErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}