using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Service.Queries.GetAllServices;

public class GetAllServicesQueryHandler : IQueryHandler<GetAllServicesQuery, List<ServiceDto>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllServicesQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Result<List<ServiceDto>>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await _serviceRepository.GetAll(cancellationToken);
    }
}