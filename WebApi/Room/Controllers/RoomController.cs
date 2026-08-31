using Application.Room.Commands.CreateRoom;
using Application.Room.Commands.DeleteRoomById;
using Application.Room.Commands.UpdateRoomById;
using Application.Room.Queries.GetAllRooms;
using Application.Room.Queries.GetRoomById;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Constants;
using WebApi.Room.Requests;
using WebApi.Room.Responses;

namespace WebApi.Room.Controllers;

[ApiController]
[Route("api/v1/rooms")]
public class RoomController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request, CancellationToken cancellationToken)
    {
        CreateRoomCommand command = new(request.Name, request.Capacity, request.BaseRentalRate, request.Services);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<RoomResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRooms(CancellationToken cancellationToken)
    {
        GetAllRoomsQuery query = new();
        var result = await Sender.Send(query, cancellationToken);

        List<RoomResponse> response = result.Value
            .Select(room => new RoomResponse
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                BaseRentalRate = room.BaseRentalRate,
                Services = room.Services
            }).ToList();
        
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(RoomResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        GetRoomByIdQuery query = new(id);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        var room = result.Value;
        RoomResponse response = new()
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            BaseRentalRate = room.BaseRentalRate,
            Services = room.Services
        };

        return Ok(response);
    }

    [HttpPut]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom([FromRoute] Guid id, [FromBody] UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        UpdateRoomByIdCommand command = new(id, request.Name, request.Capacity, request.BaseRentalRate, request.Services);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return result.Error.Code == RoomErrors.Codes.NotFound 
            ? NotFound(result.Error) 
            : BadRequest(result.Error);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoom(Guid id, CancellationToken cancellationToken)
    {
        DeleteRoomByIdCommand command = new(id);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}