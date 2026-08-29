namespace WebApi.Service.Responses;

public class ServiceResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}