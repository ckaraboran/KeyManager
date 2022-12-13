using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using KeyManager.Api.DTOs.Requests;
using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace KeyManager.Api.Controllers;

/// <summary>
///     Endpoint for account management
/// </summary>
[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ISender _mediator;

    public AccountController(IConfiguration config, ISender mediator)
    {
        _config = config;
        _mediator = mediator;
    }

    /// <summary>
    ///     Login endpoint for users
    /// </summary>
    /// <param name="userLoginRequest"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [AllowAnonymous]
    [HttpPost("login")]
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

    /// <summary>
    ///     Creates a token for the user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
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

    /// <summary>
    ///     Authenticates the user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private async Task<UserWithRolesDto> Authenticate(string username, string password)
    {
        var userAuthenticated = await _mediator.Send(new CheckAuthenticationCommand(username, password));
        if (!userAuthenticated) return null;
        var currentUser = await _mediator.Send(new GetUserRolesByUsername(username));
        return currentUser;
    }

    /// <summary>
    ///     Updates the password for the current user
    /// </summary>
    /// <param name="updateUserPasswordRequest"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(AuthorizationRequirement))]
    [HttpPost("password")]
    public async Task<ActionResult>
        UpdatePasswordAsync([FromBody] UpdateUserPasswordRequest updateUserPasswordRequest)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return Unauthorized();
        await _mediator.Send(new UpdateUserPasswordCommand(username,
            updateUserPasswordRequest.OldPassword, updateUserPasswordRequest.NewPassword));
        return Ok();
    }

    /// <summary>
    ///     Lists all roles for the current user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("roles")]
    [Authorize(Policy = nameof(KnownRolesRequirement))]
    public IActionResult AdminEndPoint()
    {
        var roles = GetCurrentUserRoles();
        return Ok($"Hi. you have these roles: {string.Join(", ", roles)}");
    }

    /// <summary>
    ///     Returns the current user roles
    /// </summary>
    /// <returns>current user roles</returns>
    private IEnumerable<string> GetCurrentUserRoles()
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity) return new List<string>();
        var userClaims = identity.Claims.ToList();
        return userClaims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value.ToString()).ToList();
    }
}