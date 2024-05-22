

namespace SurveyBasket.API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollServices pollSevice) : ControllerBase
{
  
    private readonly IPollServices _pollSevice = pollSevice;

   
    [HttpGet("")] //api/Polls
  
    public async Task< IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollSevice.GetAllAsync(cancellationToken);

        
        return Ok(polls);

    }

    [HttpGet("{id}")] //api/Polls/{id}
    
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollSevice.GetAsync(id,cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();

    }


    [HttpPost("")] //api/Polls
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {


        var newPoll = await _pollSevice.AddAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
       
    }

    [HttpPut("{id}")] //api/Polls/{id}
    public async Task< IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result = await _pollSevice.UpdateAsync(id, request,cancellationToken);


        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();

    }

    [HttpDelete("{id}")] //api/Polls/{id}
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken )
    {
        var result = await _pollSevice.DeleteAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/toggleIsPublish")] //api/Polls/{id}/toggleIsPublish
    public async Task<IActionResult> ToggleIsPublish([FromRoute] int id ,CancellationToken cancellationToken)
    {
        var result  = await _pollSevice.ToggleIsPublishedAsync(id, cancellationToken);

        return result.IsSuccess
               ? NoContent()
               : result.ToProblem();
    }

}
