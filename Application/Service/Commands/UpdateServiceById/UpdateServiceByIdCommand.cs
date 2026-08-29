using Application.Abstractions;

namespace Application.Service.Commands.UpdateServiceById;

public record struct UpdateServiceByIdCommand(Guid Id, string Name, decimal Price) : ICommand;