using System.ComponentModel.DataAnnotations;
using Domain.ValueObjects.Service;

namespace WebApi.Service.Requests;

public class CreateServiceRequest
{
    [Required, StringLength(ServiceName.MaxLength, MinimumLength = 1)] public required string Name { get; init; }
    [Required, Range(0, double.MaxValue)] public required decimal Price { get; init; }
}