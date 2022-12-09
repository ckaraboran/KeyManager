using Boilerplate.Application.Commands;
using Boilerplate.Application.Queries;
using MediatR;

namespace Boilerplate.Api.Controllers;

[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/dummy")]
[ApiController]
public class DummyController : Controller
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public DummyController(ISender mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetDummyResponse>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<GetDummyResponse>))]
    [HttpGet]
    public async Task<ActionResult<List<GetDummyResponse>>> GetAsync()
    {
        var result = await _mediator.Send(new GetAllDummiesQuery());
        return Ok(_mapper.Map<List<GetDummyResponse>>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDummyResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GetDummyResponse))]
    [HttpGet("{id}")]
    public async Task<ActionResult<GetDummyResponse>> GetAsync(int id)
    {
        var result = await _mediator.Send(new GetDummyQuery(id));
        return Ok(_mapper.Map<GetDummyResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateDummyResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CreateDummyResponse))]
    [HttpPost]
    public async Task<ActionResult<CreateDummyResponse>> PostAsync([FromBody] CreateDummyCommand createDummyCommand)
    {
        var result = await _mediator.Send(createDummyCommand);
        return Created(nameof(PostAsync), _mapper.Map<CreateDummyResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateDummyResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateDummyResponse))]
    [HttpPut]
    public async Task<ActionResult<UpdateDummyResponse>> PutAsync([FromBody] UpdateDummyCommand updateDummyCommand)
    {
        var result = await _mediator.Send(updateDummyCommand);
        return Ok(_mapper.Map<UpdateDummyResponse>(result));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(void))]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteDummyCommand(id));
        return Ok();
    }
}