using Domain.Entities;

namespace Domain.Abstractions;

public interface IRoomRepository
{
    public void Add(RoomDto room);
    
    public Task<List<RoomDto>> GetAll(CancellationToken cancellationToken);
    public Task<RoomDto?> GetById(Guid id, CancellationToken cancellationToken);
    
    public Task<bool> TryUpdate(RoomDto updatedRoom, CancellationToken cancellationToken);
    public Task<bool> TryDelete(Guid id, CancellationToken cancellationToken);
}