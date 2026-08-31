using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects.Service;

namespace Application.Service.Commands.UpdateServiceById;

public class UpdateServiceByIdCommandHandler : ICommandHandler<UpdateServiceByIdCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceByIdCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateServiceByIdCommand request, CancellationToken cancellationToken)
    {
        var serviceNameResult = ServiceName.Create(request.Name);
        if (serviceNameResult.IsFailure)
        {
            return Result.Failure<Guid>(serviceNameResult.Error);
        }

        var servicePriceResult = ServicePrice.Create(request.Price);
        if (servicePriceResult.IsFailure)
        {
            return Result.Failure<Guid>(servicePriceResult.Error);
        }

        ServiceSnapshot newService = new(request.Id)
        {
            Name = serviceNameResult.Value,
            Price = servicePriceResult.Value
        };
        
        bool updated = await _serviceRepository.TryUpdate(newService, cancellationToken);
        if (!updated)
        {
            return Result.Failure(ServiceErrors.Helpers.NotFound(request.Id));
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success(request.Id);
    }
}