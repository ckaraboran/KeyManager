using System.Linq;
using System.Security.Claims;

namespace KeyManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[ExcludeFromCodeCoverage]
public class UserController : ControllerBase
{
    //For admin Only
    [HttpGet]
    [Route("Admins")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEndPoint()
    {
        var currentUser = GetCurrentUser();
        return Ok($"Hi you have these roles: {string.Join(", ", currentUser.RoleNames)}");
    }
    private UserWithRolesDto GetCurrentUser()
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity) return null;
        var userClaims = identity.Claims.ToList();
        return new UserWithRolesDto
        {
            Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
            RoleNames = userClaims.Select(x => x.Type == ClaimTypes.Role).Select(x=>x.ToString()).ToList()
        };
    }
}