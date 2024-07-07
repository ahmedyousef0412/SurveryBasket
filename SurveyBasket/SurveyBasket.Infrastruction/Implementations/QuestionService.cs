

using Microsoft.Extensions.Logging;
using SurveyBasket.Contracts.Answers;
using SurveyBasket.Contracts.Questions;


namespace SurveyBasket.Infrastruction.Implementations;
internal class QuestionService(ApplicationDbContext context ,ICacheService cacheService ,ILogger<QuestionService> logger) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<QuestionService> _logger = logger;
    private const string _cachePrefix = "availableQuestions";

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollIsExist = await _context.Polls.AnyAsync(p => p.Id ==pollId ,cancellationToken);

        if (!pollIsExist)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

        var questions = await _context.Questions
            .Where(q => q.PollId == pollId)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {

        // Check if user vote on this before.
        //var hasVotedBefor = await _context.Votes
        //    .AnyAsync(
        //    v => v.PollId == pollId
        //     && v.UserId == userId
        //     , cancellationToken
        //    );

        //if (hasVotedBefor)
        //    return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);


        ////Check if poll is exsit.
        //var pollIsExsit = await _context.Polls
        //    .AnyAsync(
        //               p =>p.Id == pollId
        //               &&  p.IsPublished
        //               && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
        //               && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
        //               , cancellationToken
        //    );


        //if (!pollIsExsit)
        //    return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);

        var cacheKey = $"{_cachePrefix}-{pollId}";


        var cachedQuestion = await _cacheService.GetAsync<IEnumerable<QuestionResponse>>(cacheKey,cancellationToken);

        IEnumerable<QuestionResponse> questions = [];

        //Response

        if (cachedQuestion is null)
        {

            _logger.LogInformation("Select questions from database");

            questions = await _context.Questions

             .Where(q => q.PollId == pollId && q.IsActive)
             .Include(q => q.Answers)
             //Projection the QuestionResponse
             .Select(q => new QuestionResponse(
                 q.Id
                 , q.Content
                 , q.Answers.Where(a => a.IsActive)
                 //Projection the AnswerResponse
                 .Select(a => new AnswerResponse(a.Id, a.Content))
                 ))
             .AsNoTracking()
             .ToListAsync(cancellationToken);

            await _cacheService.SetAsync(cacheKey, questions, cancellationToken);
        }
        else
        {
            _logger.LogInformation("Get questions from cache");

            questions = cachedQuestion;
        }
        

        return Result.Success<IEnumerable<QuestionResponse>>(questions);

    }
    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var questions = await _context.Questions
            .Where(q => q.PollId == pollId && q.Id == id)
            .Include(q => q.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        if (questions is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success(questions);
    }
   
    
    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
    {

        // Check if any poll exist with the same pollId  to create question under it?
        var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId ,cancellationToken);


        if (!pollIsExists)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);


        //Check if any question  exist with the same content and under the same pollId?
        var questionIsExists = await _context.Questions
            .AnyAsync(q => q.Content == request.Content && q.PollId == pollId ,cancellationToken);


        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        //Mapping
        var question = request.Adapt<Question>();
        question.PollId = pollId;


        await _context.AddAsync(question,cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"{_cacheService}-{pollId}",cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());

    }

    public async Task<Result> UpdateAsync(int pollId,int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var questionIsExist = await _context.Questions
                                          .AnyAsync(q => q.PollId == pollId
                                          && q.Id != id
                                          && q.Content == request.Content,
                                          cancellationToken);

        if(questionIsExist)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        var question = await _context.Questions
            .Include(q => q.Answers)
            .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);


        question.Content = request.Content;


        //Current Answers
        var currentAnswers = question.Answers.Select(a => a.Content);


        //New Answers that not exsit in db
        var newAnswers = request.Answers.Except(currentAnswers).ToList();

        newAnswers.ForEach(answer => {
            question.Answers.Add(new Answer { Content = answer });
        });


        //If I already have the answers and comming again with request 
        // Check if isActive is false then will make it true
        //And If I remove answer that was exsit in db then will make isActive false

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);

        });


        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"{_cacheService}-{pollId}", cancellationToken);

        return Result.Success();


    }
   
    
    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == id, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"{_cacheService}-{pollId}", cancellationToken);

        return Result.Success();
    }

   
}
