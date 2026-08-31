using Application.Abstractions;
using Domain.Entities;

namespace Application.Service.Queries.GetServiceById;

public record struct GetServiceByIdQuery(Guid Id) : IQuery<ServiceSnapshot>;