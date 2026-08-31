using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Abstractions;
using WebApi.Configurations;
using WebApi.Helpers;
using WebApi.User.Requests;

namespace WebApi.User.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(ISender sender, IConfiguration configuration) : ApiController(sender)
{
    [HttpPost]
    [Route("create-token")]
    public async Task<IActionResult> CreateToken([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        Guid id = Guid.NewGuid();
        
        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, request.Username),
            new(ClaimTypes.Role, request.Role)
        ];
        
        var jwtConfiguration = configuration.GetEntry<JwtConfigurationEntry>();
        
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: jwtConfiguration.Issuer,
            audience: jwtConfiguration.Audience,
            claims: claims,
            expires: null,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { Token = tokenString });
    }
}