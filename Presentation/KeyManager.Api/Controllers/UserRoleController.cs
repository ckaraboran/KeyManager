using KeyManager.Api.DTOs.Responses.UserRoles;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Application.Queries.UserRoles;
using MediatR;

namespace KeyManager.Api.Controllers;

/// <summary>
///     Endpoint for managing user roles
/// </summary>
[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/userroles")]
[ApiController]
public class UserRoleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public UserRoleController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    ///     Get all user roles
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetUserRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetUserRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    [HttpGet]
    public async Task<ActionResult<List<GetUserRoleResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetUserRolesQuery());
        return Ok(_mapper.Map<List<GetUserRoleResponse>>(result));
    }

    /// <summary>
    ///     Create a new user role
    /// </summary>
    /// <param name="createUserRoleCommand"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUserRoleResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CreateUserRoleResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPost]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<CreateUserRoleResponse>> PostAsync(
        [FromBody] CreateUserRoleCommand createUserRoleCommand)
    {
        var result = await _mediator.Send(createUserRoleCommand);
        return Created(nameof(PostAsync), _mapper.Map<CreateUserRoleResponse>(result));
    }

    /// <summary>
    ///     Delete a user role
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Deletion result</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpDelete("{id}")]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteUserRoleCommand(id));
        return Ok();
    }
}