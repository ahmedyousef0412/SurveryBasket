
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Contracts.Polls;

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

        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);

    }

    [HttpGet("{id}")] //api/Polls/{id}
    
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await _pollSevice.GetAsync(id,cancellationToken);

        var response = poll.Adapt<PollResponse>();

        return poll is null ? NotFound() : Ok(response);
    }


    [HttpPost("")] //api/Polls
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {

        
        var newPoll = await _pollSevice.AddAsync(request.Adapt<Poll>(),cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
       
    }

    [HttpPut("{id}")] //api/Polls/{id}
    public async Task< IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollSevice.UpdateAsync(id, request.Adapt<Poll>(),cancellationToken);

        if (!isUpdated)
            return NotFound();


        return NoContent();


    }

    [HttpDelete("{id}")] //api/Polls/{id}
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken )
    {
        var isDeleted =await _pollSevice.DeleteAsync(id,cancellationToken);

        if (!isDeleted)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/toggleIsPublish")] //api/Polls/{id}/toggleIsPublish
    public async Task<IActionResult> ToggleIsPublish([FromRoute] int id ,CancellationToken cancellationToken)
    {
        var isToggle = await _pollSevice.ToggleIsPublishedAsync(id, cancellationToken);

        if(!isToggle)
            return NotFound();

        return NoContent();
    }

}
