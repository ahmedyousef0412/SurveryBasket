﻿



namespace SurveyBasket.API.Controllers;

[Route("api/polls/{pollId}/[controller]")]
[ApiController]

public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet("")]
    [HasPermission(Permessions.GetQuestions)]
    public async Task<IActionResult> GetAll([FromRoute] int pollId ,CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAllAsync(pollId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("{id}")]
    [HasPermission(Permessions.GetQuestions)]

    public async Task<IActionResult> Get([FromRoute] int pollId , [FromRoute] int id ,CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAsync(pollId ,id , cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPost("")]
    [HasPermission(Permessions.AddQuestions)]

    public async Task<IActionResult> Add([FromRoute] int pollId , [FromBody] QuestionRequest request , CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(pollId,request,cancellationToken);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);
        

         return result.ToProblem();
    }


    [HttpPut("{id}")]
    [HasPermission(Permessions.UpdateQuestions)]

    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdateAsync(pollId,id, request, cancellationToken);

        if (result.IsSuccess)
            return NoContent();


        return result.ToProblem();
    }

  
    
    [HttpPut("{id}/toggleStatus")]
    [HasPermission(Permessions.UpdateQuestions)]

    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
