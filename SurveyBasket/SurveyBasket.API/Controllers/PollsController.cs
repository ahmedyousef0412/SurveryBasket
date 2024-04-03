
namespace SurveyBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollServices pollSevice) : ControllerBase
{
  
    private readonly IPollServices _pollSevice = pollSevice;


    [HttpGet("")]
    public IActionResult GetAll()
    {
        var polls = _pollSevice.GetAll();

        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);

    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollSevice.Get(id);

        var response = poll.Adapt<PollResponse>();

        return poll is null ? NotFound() : Ok(response);
    }


    [HttpPost("")]
    public IActionResult Add([FromBody] CreatePollRequest request )
    {

        
        var newPoll = _pollSevice.Add(request.Adapt<Poll>());

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
       
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] CreatePollRequest request)
    {
        var isUpdated = _pollSevice.Update(id, request.Adapt<Poll>());

        if (!isUpdated)
            return NotFound();


        return NoContent();


    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var isDeleted = _pollSevice.Delete(id);

        if (!isDeleted)
            return NotFound();

        return NoContent();
    }

}
