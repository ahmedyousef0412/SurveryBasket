


namespace SurveyBasket.API.Controllers;
[Route("api/Polls/{pollId}/vote")]
[Authorize(Roles = DefaultRoles.Member)]
[ApiController]
public class VotesController(IQuestionService questionService,IVoteServices voteServices) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteServices _voteServices = voteServices;

    [HttpGet("")]
    public async Task<IActionResult> StartVoting([FromRoute] int pollId , CancellationToken cancellationToken)
    {


        var result = await _questionService.GetAvailableAsync(pollId, User.GetUserId()!, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value) 
            : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute]int pollId, [FromBody] VoteRequest request ,  CancellationToken cancellationToken)
    {
        var result = await _voteServices.AddAsync(pollId,User.GetUserId()!, request , cancellationToken);

        return result.IsSuccess 
            ? Created() 
            : result.ToProblem();
    }
}
