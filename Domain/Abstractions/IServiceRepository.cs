using Domain.Entities;

namespace Domain.Abstractions;

public interface IServiceRepository
{
    public void Add(ServiceSnapshot service);
    
    public Task<List<ServiceSnapshot>> GetAll(CancellationToken cancellationToken);
    public Task<ServiceSnapshot?> GetById(Guid id, CancellationToken cancellationToken);
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    
    public Task<bool> TryUpdate(ServiceSnapshot updatedService, CancellationToken cancellationToken);
    public Task<bool> TryDelete(Guid id, CancellationToken cancellationToken);
}