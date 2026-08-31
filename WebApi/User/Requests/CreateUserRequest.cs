using System.ComponentModel.DataAnnotations;

namespace WebApi.User.Requests;

public class CreateUserRequest
{
    [Required] public string Username { get; init; }
    [Required] public string Role { get; init; }
}