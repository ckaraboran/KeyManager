using KeyManager.Api.DTOs.Responses.Incident;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Queries.Incidents;
using MediatR;

namespace KeyManager.Api.Controllers;

/// <summary>
///     Endpoint for incidents
/// </summary>
[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/incidents")]
[ApiController]
public class IncidentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public IncidentController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    ///     Get all incidents
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetIncidentResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetIncidentResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [Authorize(Policy = nameof(UserManagerRequirement))]
    [HttpGet]
    public async Task<ActionResult<List<GetIncidentResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetIncidentsQuery());
        return Ok(_mapper.Map<List<GetIncidentResponse>>(result));
    }
}