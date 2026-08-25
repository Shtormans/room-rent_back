using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Abstractions;

public abstract class ApiController(ISender sender) : Controller
{
    protected readonly ISender Sender = sender;
}