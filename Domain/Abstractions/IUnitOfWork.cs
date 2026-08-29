namespace Domain.Abstractions;

public interface IUnitOfWork
{
    public Task SaveChanges(CancellationToken cancellationToken);
}