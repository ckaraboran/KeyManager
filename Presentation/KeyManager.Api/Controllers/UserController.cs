using System.Linq;
using System.Security.Claims;
using KeyManager.Api.DTOs.Requests;
using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;

namespace KeyManager.Api.Controllers;

[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public UserController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    //For admin Only
    [HttpGet]
    [Route("Roles")]
    [Authorize(Policy = nameof(KnownRolesRequirement))]
    public IActionResult AdminEndPoint()
    {
        var roles = GetCurrentUserRoles();
        return Ok($"Hi. you have these roles: {string.Join(", ", roles)}");
    }

    private List<string> GetCurrentUserRoles()
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity) return new List<string>();
        var userClaims = identity.Claims.ToList();
        return userClaims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value.ToString()).ToList();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetUserResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetUserResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpGet]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<List<GetUserResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetUsersQuery());
        return Ok(_mapper.Map<List<GetUserResponse>>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GetUserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpGet("{id}")]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<GetUserResponse>> GetAsync(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(_mapper.Map<GetUserResponse>(result));
    }


    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUserResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CreateUserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPost]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<CreateUserResponse>> PostAsync([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await _mediator.Send(createUserCommand);
        return Created(nameof(PostAsync), _mapper.Map<CreateUserResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<UpdateUserResponse>> PutAsync([FromBody] UpdateUserCommand updateDummyCommand)
    {
        var result = await _mediator.Send(updateDummyCommand);
        return Ok(_mapper.Map<UpdateUserResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut("password")]
    public async Task<ActionResult>
        UpdatePasswordAsync([FromBody] UpdateUserPasswordRequest updateUserPasswordRequest)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return Unauthorized();
        await _mediator.Send(new UpdateUserPasswordCommand(username,
            updateUserPasswordRequest.OldPassword, updateUserPasswordRequest.NewPassword));
        return Ok();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return Ok();
    }
}