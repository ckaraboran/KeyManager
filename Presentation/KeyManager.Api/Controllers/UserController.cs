using KeyManager.Api.DTOs.Responses.Users;
using KeyManager.Api.Security.Requirements;
using KeyManager.Application.Commands.Users;
using KeyManager.Application.Queries.Users;
using MediatR;

namespace KeyManager.Api.Controllers;

/// <summary>
///     Endpoint for managing users
/// </summary>
[Authorize(Policy = nameof(AuthorizationRequirement))]
[Route("api/users")]
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

    /// <summary>
    ///     Get all users
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    ///     Get a user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>User with the specific id</returns>
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

    /// <summary>
    ///     Create a new user
    /// </summary>
    /// <param name="createUserCommand"></param>
    /// <returns>Created user</returns>
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

    /// <summary>
    ///     Update a user
    /// </summary>
    /// <param name="updateUserCommand"></param>
    /// <returns>Updated user</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(UpdateUserResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(void))]
    [HttpPut]
    [Authorize(Policy = nameof(SystemManagerRequirement))]
    public async Task<ActionResult<UpdateUserResponse>> PutAsync([FromBody] UpdateUserCommand updateUserCommand)
    {
        var result = await _mediator.Send(updateUserCommand);
        return Ok(_mapper.Map<UpdateUserResponse>(result));
    }

    /// <summary>
    ///     Delete a user
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Deletion result</returns>
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