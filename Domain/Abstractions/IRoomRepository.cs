using Domain.Entities;

namespace Domain.Abstractions;

public interface IRoomRepository
{
    public void Add(RoomSnapshot room);
    
    public Task<List<RoomSnapshot>> GetAll(CancellationToken cancellationToken);
    public Task<RoomSnapshot?> GetById(Guid id, CancellationToken cancellationToken);
    
    public Task<bool> TryUpdate(RoomSnapshot updatedRoom, CancellationToken cancellationToken);
    public Task<bool> TryDelete(Guid id, CancellationToken cancellationToken);
}