
namespace SurveyBasket.API.Controllers;


[Route("api/[controller]")]
[ApiController]

public class PollsController(IPollServices pollSevice) : ControllerBase
{
  
    private readonly IPollServices _pollSevice = pollSevice;

   
    [HttpGet("")] //api/Polls
    [HasPermission(Permessions.GetPolls)]
    public async Task< IActionResult> GetAll(CancellationToken cancellationToken)
    {

        return Ok(await _pollSevice.GetAllAsync(cancellationToken));
    }

   
    [HttpGet("current")] //api/Polls/current
    [Authorize(Roles  =DefaultRoles.Member)]
    [EnableRateLimiting(Policies.UserLimit)]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {

        return Ok(await _pollSevice.GetCurrentAsync(cancellationToken));
    }


    [HttpGet("{id}")] //api/Polls/{id} 
    [HasPermission(Permessions.GetPolls)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollSevice.GetAsync(id,cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();

    }


    [HttpPost("")] //api/Polls
    [HasPermission(Permessions.AddPolls)]
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {


        var result = await _pollSevice.AddAsync(request, cancellationToken);

        return result.IsSuccess
               ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
               :result.ToProblem();

       
       
    }

   
    
    [HttpPut("{id}")] //api/Polls/{id}
    [HasPermission(Permessions.UpdatePolls)]
    public async Task< IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollSevice.UpdateAsync(id, request,cancellationToken);

        
        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();

    }

   
    
    [HttpDelete("{id}")] //api/Polls/{id}
    [HasPermission(Permessions.DeletePolls)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken )
    {
        var result = await _pollSevice.DeleteAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

   
    
    [HttpPut("{id}/toggleIsPublish")] //api/Polls/{id}/toggleIsPublish
    [HasPermission(Permessions.UpdatePolls)]
    public async Task<IActionResult> ToggleIsPublish([FromRoute] int id ,CancellationToken cancellationToken)
    {
        var result  = await _pollSevice.ToggleIsPublishedAsync(id, cancellationToken);

        return result.IsSuccess
               ? NoContent()
               : result.ToProblem();
    }

}
