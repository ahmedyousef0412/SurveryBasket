


namespace SurveyBasket.Infrastruction.Implementations;
internal class ResultService(ApplicationDbContext context) : IResultService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken)
    {
        var pollVotes = await _context.Polls
            .Where(p => p.Id == pollId)
            .Select(x => new PollVotesResponse(
              x.Title,
              x.Votes.Select(v => new VoteResponse(
                 $"{v.User.FirstName} {v.User.LastName}",
                 v.SubmittedOn,
                 v.VoteAnswers.Select(a => new QuestionAnswerResponse(
                   a.Question.Content,
                   a.Answer.Content
                 ))
              ))
            )).SingleOrDefaultAsync(cancellationToken);

        return pollVotes is null
            ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
            : Result.Success(pollVotes);

    }

    public async Task<Result<IEnumerable<VotePerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken)
    {
        var pollIsExsits = await PollIsExsits(pollId, cancellationToken);

        if (!pollIsExsits)
            return Result.Failure< IEnumerable<VotePerDayResponse>>(PollErrors.PollNotFound);

        var votesPerDay = await _context.Votes.Where(v => v.PollId == pollId)
            .GroupBy(vote =>
                          new { Date = DateOnly.FromDateTime(vote.SubmittedOn) }
            )
            .Select(g => new VotePerDayResponse
            ( 
                g.Key.Date,
                g.Count()
            ))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotePerDayResponse>>(votesPerDay);
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken)
    {
        var pollIsExsits = await PollIsExsits(pollId, cancellationToken);

        if (!pollIsExsits)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);



        var votesPerQuestion = await _context.VoteAnswers
            .Where(x => x.Vote.PollId == pollId)

            .Select(x => new VotesPerQuestionResponse
            (
                x.Question.Content,
                x.Question.VoteAnswers

                .GroupBy
                (
                    x => new { AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content }
                )
                .Select
                (
                    g => new VotesPerAnswerResponse
                    (
                        g.Key.AnswerContent,
                        g.Count()
                    )
                )

            )

            ).ToListAsync(cancellationToken);


        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);

    }


    private async Task<bool> PollIsExsits(int pollId, CancellationToken cancellationToken =default)
    {
        return await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken);
    }
}
