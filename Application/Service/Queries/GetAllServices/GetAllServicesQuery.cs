using Application.Abstractions;
using Domain.Entities;

namespace Application.Service.Queries.GetAllServices;

public record struct GetAllServicesQuery : IQuery<List<ServiceDto>>;