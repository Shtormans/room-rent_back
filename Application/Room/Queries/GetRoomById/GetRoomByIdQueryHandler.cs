using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Room.Queries.GetRoomById;

public class GetRoomByIdQueryHandler : IQueryHandler<GetRoomByIdQuery, RoomDto>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<RoomDto>> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetById(request.Id, cancellationToken);

        if (room is null)
        {
            return Result.Failure<RoomDto>(RoomErrors.Helpers.NotFound(request.Id));
        }

        return room;
    }
}