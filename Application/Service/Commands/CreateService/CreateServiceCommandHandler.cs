using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects.Service;

namespace Application.Service.Commands.CreateService;

public class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand, Guid>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
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

        ServiceDto service = ServiceDto.Create(serviceNameResult.Value, servicePriceResult.Value);
        
        _serviceRepository.Add(service);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return service.Id;
    }
}