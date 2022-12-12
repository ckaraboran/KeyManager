using KeyManager.Api.DTOs.Responses.Permissions;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Permissions;
using KeyManager.Application.Queries.Permissions;
using MediatR;

namespace KeyManager.Api.Controllers;

[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/{controller}")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public PermissionController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetPermissionResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetPermissionResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    [HttpGet]
    public async Task<ActionResult<List<GetPermissionResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetPermissionsQuery());
        return Ok(_mapper.Map<List<GetPermissionResponse>>(result));
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePermissionResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CreatePermissionResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPost]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<CreatePermissionResponse>> PostAsync(
        [FromBody] CreatePermissionCommand createPermissionCommand)
    {
        var result = await _mediator.Send(createPermissionCommand);
        return Created(nameof(PostAsync), _mapper.Map<CreatePermissionResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdatePermissionResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdatePermissionResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<UpdatePermissionResponse>> PutAsync(
        [FromBody] UpdatePermissionCommand updatePermissionCommand)
    {
        var result = await _mediator.Send(updatePermissionCommand);
        return Ok(_mapper.Map<UpdatePermissionResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpDelete("{id}")]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeletePermissionCommand(id));
        return Ok();
    }
}