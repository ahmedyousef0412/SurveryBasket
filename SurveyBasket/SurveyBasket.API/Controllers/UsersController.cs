

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    [HasPermission(Permessions.GetUsers)]
    public async Task<IActionResult> GetAll(CancellationToken cancellation)
    {
        return Ok(await _userService.GetAllAsync(cancellation));

    }

    [HttpGet("{id}")]
    [HasPermission(Permessions.GetUsers)]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var result = await _userService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("")]
    [HasPermission(Permessions.AddUsers)]
    public async Task<IActionResult> Add([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.AddAsync(request, cancellationToken);

        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value) : result.ToProblem();

    }

    [HttpPut("{id}")]
    [HasPermission(Permessions.UpdateUsers)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }
    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permessions.UpdateUsers)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await _userService.ToggleStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }
    [HttpPut("{id}/unlock")]
    [HasPermission(Permessions.UpdateUsers)]
    public async Task<IActionResult> Unlock([FromRoute] string id)
    {
        var result = await _userService.Unlock(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }
}
