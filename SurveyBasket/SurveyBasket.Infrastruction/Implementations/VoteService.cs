

namespace SurveyBasket.Infrastruction.Implementations;
public class VoteService(ApplicationDbContext context) : IVoteServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
    {
        // Check if user vote on this before.
        var hasVotedBefor = await _context.Votes
            .AnyAsync(
            v => v.PollId == pollId
             && v.UserId == userId
             , cancellationToken
            );

        if (hasVotedBefor)
            return Result.Failure(VoteErrors.DuplicatedVote);


        //Check if poll is exsit.
        var pollIsExsit = await _context.Polls
            .AnyAsync(
                       p => p.Id == pollId
                       && p.IsPublished
                       && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                       && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
                       , cancellationToken
            );


        if (!pollIsExsit)
            return Result.Failure(PollErrors.PollNotFound);

        // This code retrieves a list of question IDs from the database.

        var avaliableQuestions = await _context.Questions
            .Where(q => q.PollId == pollId && q.IsActive)
            .Select(q => q.Id)
            .ToListAsync(cancellationToken);

        // The 'availableQuestions' variable will now contain a list of integer IDs representing the available questions 
        // that meet the specified criteria (belong to the given pollId and are active).



        //Compare questions Id that will come from request with questions Id in Db
        if (!request.Answers.Select(var => var.QuestionId).SequenceEqual(avaliableQuestions))
            return Result.Failure(VoteErrors.InvalidQuestions);

        var vote = new Vote
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };

        await _context.Votes.AddAsync(vote, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
