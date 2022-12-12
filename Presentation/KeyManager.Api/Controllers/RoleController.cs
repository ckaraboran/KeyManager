using KeyManager.Api.DTOs.Responses.Roles;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Roles;
using KeyManager.Application.Queries.Roles;
using MediatR;

namespace KeyManager.Api.Controllers;

public class RoleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public RoleController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetRoleResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(KnownRolesRequirement))]
    [HttpGet]
    public async Task<ActionResult<List<GetRoleResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetRolesQuery());
        return Ok(_mapper.Map<List<GetRoleResponse>>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRoleResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GetRoleResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpGet("{id}")]
    [Authorize(Policy = nameof(KnownRolesRequirement))]
    public async Task<ActionResult<GetRoleResponse>> GetAsync(int id)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery(id));
        return Ok(_mapper.Map<GetRoleResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateRoleResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CreateRoleResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPost]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<CreateRoleResponse>> PostAsync([FromBody] CreateRoleCommand createRoleCommand)
    {
        var result = await _mediator.Send(createRoleCommand);
        return Created(nameof(PostAsync), _mapper.Map<CreateRoleResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateRoleResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateRoleResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<UpdateRoleResponse>> PutAsync([FromBody] UpdateRoleCommand updateRoleCommand)
    {
        var result = await _mediator.Send(updateRoleCommand);
        return Ok(_mapper.Map<UpdateRoleResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpDelete("{id}")]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return Ok();
    }
}