using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KeyManager.Api.DTOs.Requests;
using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace KeyManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ISender _mediator;

    public LoginController(IConfiguration config, ISender mediator)
    {
        _config = config;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        var user = await Authenticate(userLoginRequest.Username, userLoginRequest.Password);
        if (user != null)
        {
            var token = GenerateToken(user);

            return Ok(new UserTokenResponse(token));
        }

        return NotFound("user not found");
    }

    // To generate token
    private string GenerateToken(UserWithRolesDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim> { new(ClaimTypes.Name, user.Username) };
        foreach (var roleName in user.RoleNames) claims.Add(new Claim(ClaimTypes.Role, roleName));

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //To authenticate user
    private async Task<UserWithRolesDto> Authenticate(string username, string password)
    {
        var userAuthenticated = await _mediator.Send(new CheckAuthenticationCommand(username, password));
        if (!userAuthenticated) return null;
        var currentUser = await _mediator.Send(new GetUserRolesByUsername(username));
        return currentUser;
    }
}