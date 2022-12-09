using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Boilerplate.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthUsersRepository _authUsersRepository;
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config, IAuthUsersRepository authUsersRepository)
    {
        _config = config;
        _authUsersRepository = authUsersRepository;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
    {
        var user = await Authenticate(userLogin);
        if (user != null)
        {
            var token = GenerateToken(user);
            return Ok(token);
        }

        return NotFound("user not found");
    }

    // To generate token
    private string GenerateToken(UserModel user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //To authenticate user
    private async Task<UserModel> Authenticate(UserLogin userLogin)
    {
        var currentUser = await _authUsersRepository.GetUserAsync(userLogin.Username, userLogin.Password);
        if (currentUser != null) return currentUser;
        return null;
    }
}