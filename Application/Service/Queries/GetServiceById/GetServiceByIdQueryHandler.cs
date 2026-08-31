using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Service.Queries.GetServiceById;

public class GetServiceByIdQueryHandler : IQueryHandler<GetServiceByIdQuery, ServiceSnapshot>
{
    private readonly IServiceRepository _serviceRepository;

    public GetServiceByIdQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Result<ServiceSnapshot>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetById(request.Id, cancellationToken);

        if (service is null)
        {
            return Result.Failure<ServiceSnapshot>(ServiceErrors.Helpers.NotFound(request.Id));
        }
        
        return service;
    }
}