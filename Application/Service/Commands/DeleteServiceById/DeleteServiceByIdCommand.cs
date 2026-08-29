using Application.Abstractions;

namespace Application.Service.Commands.DeleteServiceById;

public record struct DeleteServiceByIdCommand(Guid Id) : ICommand;