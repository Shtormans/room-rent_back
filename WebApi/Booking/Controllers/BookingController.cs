using Application.Booking.Commands.BookRoom;
using Application.Booking.Commands.DeleteBooking;
using Application.Booking.Queries.GetAllBookings;
using Application.Booking.Queries.GetBookingById;
using Application.Booking.Queries.SearchAvailableRooms;
using Domain.Entities;
using Domain.Shared;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Booking.Requests;
using WebApi.Booking.Responses;
using WebApi.Constants;

namespace WebApi.Booking.Controllers;

[ApiController]
[Route("api/v1/bookings")]
public class BookingController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [ProducesResponseType(typeof(List<BookingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookings(CancellationToken cancellationToken)
    {
        GetAllBookingQuery query = new();
        Result<List<BookingSnapshot>> result = await Sender.Send(query, cancellationToken);
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookingById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        GetBookingByIdQuery query = new(id);
        Result<BookingSnapshot> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        
        return Ok(result.Value);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBookingById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        DeleteBookingByIdCommand command = new(id);
        Result result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        
        return NoContent();
    }
    
    [HttpGet]
    [Route("search")]
    [ProducesResponseType(typeof(List<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAvailableRooms([FromQuery] SearchAvailableRoomsRequest request,
        CancellationToken cancellationToken)
    {
        DateTime start = DateTimeUtils.Combine(request.Date, request.StartTime);
        DateTime end = DateTimeUtils.Combine(request.Date, request.EndTime);

        SearchAvailableRoomsQuery query = new(start, end, request.Capacity);
        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("book-room")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BookRoom([FromBody] BookRoomRequest request, CancellationToken cancellationToken)
    {
        BookRoomCommand command = new(request.RoomId, request.Start, request.DurationHours, request.Services);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        BookRoomCommandResponse commandResponse = result.Value;
        BookRoomResponse response = new()
        {
            BookingId = commandResponse.BookingId,
            Price = commandResponse.Price,
        };

        return Ok(response);
    }
}