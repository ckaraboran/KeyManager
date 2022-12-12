using KeyManager.Api.DTOs.Responses.UserRoles;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.UserRoles;
using KeyManager.Application.Queries.UserRoles;
using MediatR;

namespace KeyManager.Api.Controllers;

[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/[controller]")]
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

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserRoleResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateUserRoleResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<UpdateUserRoleResponse>> PutAsync(
        [FromBody] UpdateUserRoleCommand updateUserRoleCommand)
    {
        var result = await _mediator.Send(updateUserRoleCommand);
        return Ok(_mapper.Map<UpdateUserRoleResponse>(result));
    }

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