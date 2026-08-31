using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Room.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler : IQueryHandler<GetAllRoomsQuery, List<RoomSnapshot>>
{
    private readonly IRoomRepository _roomRepository;

    public GetAllRoomsQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<List<RoomSnapshot>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _roomRepository.GetAll(cancellationToken);
    }
}