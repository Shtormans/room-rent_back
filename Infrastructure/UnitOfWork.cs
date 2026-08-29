using Domain.Abstractions;

namespace Infrastructure;

public sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}