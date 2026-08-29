using Domain.Entities;

namespace Domain.Abstractions;

public interface IServiceRepository
{
    public void Add(ServiceDto service);
    
    public Task<List<ServiceDto>> GetAll(CancellationToken cancellationToken);
    public Task<ServiceDto?> GetById(Guid id, CancellationToken cancellationToken);
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    
    public Task<bool> TryUpdate(ServiceDto updatedService, CancellationToken cancellationToken);
    public Task<bool> TryDelete(Guid id, CancellationToken cancellationToken);
}