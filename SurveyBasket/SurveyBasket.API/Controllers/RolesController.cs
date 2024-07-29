﻿

namespace SurveyBasket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    [HasPermission(Permessions.GetRoles)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled,CancellationToken cancellationToken )
    {
        var roles = await _roleService.GetAllAsync(includeDisabled ,cancellationToken);
        return Ok(roles);


    }
    [HttpGet("{id}")]
    [HasPermission(Permessions.GetRoles)]
    public async Task<IActionResult> Get([FromRoute] string id  )
    {
        var result = await _roleService.GetAsync(id );

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();


    }


    [HttpPost("")]
    [HasPermission(Permessions.AddRoles)]
    public async Task<IActionResult> Add([FromBody] RoleRequest request)
    {
        var result = await _roleService.AddAsync(request);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { result.Value!.Id }, result.Value)
            : result.ToProblem();


    }


    [HttpPut("{id}")]
    [HasPermission(Permessions.UpdateRoles)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleRequest request)
    {
        var result = await _roleService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
        
    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permessions.UpdateRoles)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await _roleService.ToggleStatusAsync(id);

        return result.IsSuccess ? NoContent(): result.ToProblem();

    }
}
