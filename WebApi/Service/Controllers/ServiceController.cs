using Application.Service.Commands.CreateService;
using Application.Service.Commands.DeleteServiceById;
using Application.Service.Commands.UpdateServiceById;
using Application.Service.Queries.GetAllServices;
using Application.Service.Queries.GetServiceById;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Constants;
using WebApi.Service.Requests;
using WebApi.Service.Responses;

namespace WebApi.Service.Controllers;

[ApiController]
[Route("api/v1/services")]
public class ServiceController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        CreateServiceCommand command = new(request.Name, request.Price);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetServices(CancellationToken cancellationToken)
    {
        GetAllServicesQuery query = new();
        var result = await Sender.Send(query, cancellationToken);

        List<ServiceResponse> response = result.Value
            .Select(service => new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name.Value,
                Price = service.Price.Value
            }).ToList();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        GetServiceByIdQuery query = new(id);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound();
        }

        var service = result.Value;
        ServiceResponse response = new()
        {
            Id = service.Id,
            Name = service.Name.Value,
            Price = service.Price.Value
        };

        return Ok(response);
    }

    [HttpPut]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateService([FromRoute] Guid id, [FromBody] UpdateServiceRequest request,
        CancellationToken cancellationToken)
    {
        UpdateServiceByIdCommand command = new(id, request.Name, request.Price);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return result.Error.Code == ServiceErrors.Codes.NotFound 
            ? NotFound(result.Error) 
            : BadRequest(result.Error);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteService([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        DeleteServiceByIdCommand command = new(id);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}