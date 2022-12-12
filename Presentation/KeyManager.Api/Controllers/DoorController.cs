using System.Security.Claims;
using KeyManager.Api.DTOs.Responses.Doors;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Doors;
using KeyManager.Application.Queries.Doors;
using MediatR;

namespace KeyManager.Api.Controllers;

[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/doors")]
[ApiController]
public class DoorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public DoorController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetDoorResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetDoorResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(KnownRolesRequirement))]
    [HttpGet]
    public async Task<ActionResult<List<GetDoorResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetDoorsQuery());
        return Ok(_mapper.Map<List<GetDoorResponse>>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDoorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GetDoorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpGet("{id}")]
    [Authorize(Policy = nameof(KnownRolesRequirement))]
    public async Task<ActionResult<GetDoorResponse>> GetAsync(int id)
    {
        var result = await _mediator.Send(new GetDoorByIdQuery(id));
        return Ok(_mapper.Map<GetDoorResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateDoorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CreateDoorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPost]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<CreateDoorResponse>> PostAsync([FromBody] CreateDoorCommand createDoorCommand)
    {
        var result = await _mediator.Send(createDoorCommand);
        return Created(nameof(PostAsync), _mapper.Map<CreateDoorResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateDoorResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateDoorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<UpdateDoorResponse>> PutAsync([FromBody] UpdateDoorCommand updateDoorCommand)
    {
        var result = await _mediator.Send(updateDoorCommand);
        return Ok(_mapper.Map<UpdateDoorResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpDelete("{id}")]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteDoorCommand(id));
        return Ok();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPost("{id}/open")]
    public async Task<ActionResult> OpenDoorAsync(int id)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        if (username == null) return Unauthorized();
        var result = await _mediator.Send(new OpenDoorCommand(username, id));
        if (!result) return Unauthorized();
        return Ok();
    }
}