using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects.Service;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ServiceRepository(ApplicationDbContext dbContext) : IServiceRepository
{
    public void Add(ServiceDto service)
    {
        Service entity = ConvertToEntity(service);
        dbContext.Set<Service>().Add(entity);
    }

    public async Task<List<ServiceDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<Service>()
            .AsNoTracking()
            .Select(service => ConvertToDto(service))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceDto?> GetById(Guid id, CancellationToken cancellationToken)
    {
        Service? service = await GetDatabaseObjectById(id, false, cancellationToken);
        return service is null ? null : ConvertToDto(service);
    }

    public async Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<Service>()
            .Where(service => service.Id == id)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> TryUpdate(ServiceDto updatedService, CancellationToken cancellationToken)
    {
        Service? service = await GetDatabaseObjectById(updatedService.Id, true, cancellationToken);
        
        if (service is null)
        {
            return false;
        }
        
        service.Name = updatedService.Name;
        service.Price = updatedService.Price;

        return true;
    }

    public async Task<bool> TryDelete(Guid id, CancellationToken cancellationToken)
    {
        Service? service = await GetDatabaseObjectById(id, false, cancellationToken);
        
        if (service is null)
        {
            return false;
        }

        dbContext.Set<Service>().Remove(service);
        return true;
    }
    
    private async Task<Service?> GetDatabaseObjectById(Guid id, bool withTracking, CancellationToken cancellationToken)
    {
        IQueryable<Service> query = dbContext.Set<Service>().AsQueryable();
        query = withTracking ? query.AsTracking() : query.AsNoTracking();
        
        return await query.FirstOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    private static ServiceDto ConvertToDto(Service entity)
    {
        var nameResult = ServiceName.Create(entity.Name);
        var priceResult = ServicePrice.Create(entity.Price);
        
        return new ServiceDto(entity.Id)
        {
            Name = nameResult.Value,
            Price = priceResult.Value
        };
    }
    
    private static Service ConvertToEntity(ServiceDto dto)
    {
        return new Service
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price
        };
    }
}