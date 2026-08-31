using Domain.Abstractions;
using Domain.Entities;
using Domain.ValueObjects.Room;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomRepository(ApplicationDbContext dbContext) : IRoomRepository
{
    public void Add(RoomSnapshot room)
    {
        Room entity = ConvertToEntity(room);
        dbContext.Set<Room>().Add(entity);
    }

    public async Task<List<RoomSnapshot>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Set<Room>()
            .AsNoTracking()
            .Include(room => room.RoomServices)
            .Select(room => ConvertToSnapshot(room))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoomSnapshot?> GetById(Guid id, CancellationToken cancellationToken)
    {
        Room? room = await GetDatabaseObjectById(id, false, cancellationToken);

        if (room == null)
        {
            return null;
        }

        return ConvertToSnapshot(room);
    }

    public async Task<bool> TryUpdate(RoomSnapshot updatedRoom, CancellationToken cancellationToken)
    {
        Room? room = await GetDatabaseObjectById(updatedRoom.Id, true, cancellationToken);

        if (room is null)
        {
            return false;
        }

        room.Name = updatedRoom.Name;
        room.Capacity = updatedRoom.Capacity;
        room.BaseRentalRate = updatedRoom.BaseRentalRate;
        room.RoomServices = updatedRoom.Services.Select(serviceId => new RoomService
        {
            RoomId = updatedRoom.Id,
            ServiceId = serviceId
        }).ToList();

        return true;
    }

    public async Task<bool> TryDelete(Guid id, CancellationToken cancellationToken)
    {
        Room? service = await GetDatabaseObjectById(id, false, cancellationToken);

        if (service is null)
        {
            return false;
        }

        dbContext.Set<Room>().Remove(service);
        return true;
    }

    private async Task<Room?> GetDatabaseObjectById(Guid id, bool withTracking, CancellationToken cancellationToken)
    {
        IQueryable<Room> query = dbContext
            .Set<Room>()
            .Include(room => room.RoomServices);
        
        query = withTracking ? query.AsTracking() : query.AsNoTracking();
        return await query.FirstOrDefaultAsync(room => room.Id == id, cancellationToken);
    }

    private static RoomSnapshot ConvertToSnapshot(Room entity)
    {
        var nameResult = RoomName.Create(entity.Name);
        var capacityResult = RoomCapacity.Create(entity.Capacity);
        var baseRentalRate = RoomRentalRate.Create(entity.BaseRentalRate);

        return new RoomSnapshot(entity.Id)
        {
            Name = nameResult.Value,
            Capacity = capacityResult.Value,
            BaseRentalRate = baseRentalRate.Value,
            Services = entity.RoomServices.Select(rs => rs.ServiceId).ToList()
        };
    }

    private static Room ConvertToEntity(RoomSnapshot snapshot)
    {
        List<RoomService> roomServices = snapshot
            .Services
            .Select(serviceId => new RoomService
            {
                RoomId = snapshot.Id,
                ServiceId = serviceId
            }).ToList();

        return new Room
        {
            Id = snapshot.Id,
            Name = snapshot.Name,
            Capacity = snapshot.Capacity,
            BaseRentalRate = snapshot.BaseRentalRate,
            RoomServices = roomServices
        };
    }
}