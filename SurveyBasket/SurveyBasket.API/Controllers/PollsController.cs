using Microsoft.AspNetCore.Http.HttpResults;
using SurveyBasket.Application.Services;
using SurveyBasket.Domain.Entities;


namespace SurveyBasket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollServices pollSevice) : ControllerBase
{
  
    private readonly IPollServices _pollSevice = pollSevice;


    [HttpGet("")]
    public IActionResult GetAll()
    {
        return Ok(_pollSevice.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var poll = _pollSevice.Get(id);
        
        return poll is null ? NotFound() : Ok(poll);
    }


    [HttpPost("")]
    public IActionResult Add(Poll  request)
    {
        var newPoll = _pollSevice.Add(request);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id , Poll request)
    {
        var isUpdated = _pollSevice.Update(id, request);

        if(!isUpdated)
            return NotFound();

        return NoContent();

    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var isDeleted = _pollSevice.Delete(id);

        if(!isDeleted)
            return NotFound();

        return NoContent();
    }

}
