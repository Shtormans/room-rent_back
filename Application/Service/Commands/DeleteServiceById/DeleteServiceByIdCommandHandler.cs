using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Service.Commands.DeleteServiceById;

public class DeleteServiceByIdCommandHandler : ICommandHandler<DeleteServiceByIdCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceByIdCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteServiceByIdCommand request, CancellationToken cancellationToken)
    {
        bool deleted = await _serviceRepository.TryDelete(request.Id, cancellationToken);
        
        if (!deleted)
        {
            return Result.Failure(ServiceErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}