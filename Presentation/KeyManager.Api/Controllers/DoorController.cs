using System.Security.Claims;
using KeyManager.Api.DTOs.Responses.Doors;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Doors;
using KeyManager.Application.Queries.Doors;
using MediatR;

namespace KeyManager.Api.Controllers;

/// <summary>
///     Endpoints for managing doors
/// </summary>
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

    /// <summary>
    ///     Get all doors
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    ///     Get door by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Door with the specific id</returns>
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

    /// <summary>
    ///     Create a new door
    /// </summary>
    /// <param name="createDoorCommand"></param>
    /// <returns>Created door</returns>
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

    /// <summary>
    ///     Update a door
    /// </summary>
    /// <param name="updateDoorCommand"></param>
    /// <returns>Updated door</returns>
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

    /// <summary>
    ///     Delete a door
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
        await _mediator.Send(new DeleteDoorCommand(id));
        return Ok();
    }

    /// <summary>
    ///     Open a door
    /// </summary>
    /// <param name="id">The door to be opened</param>
    /// <returns>Operation result</returns>
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