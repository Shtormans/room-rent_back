using Application.Abstractions;

namespace Application.Service.Commands.CreateService;

public record struct CreateServiceCommand(string Name, decimal Price) : ICommand<Guid>;