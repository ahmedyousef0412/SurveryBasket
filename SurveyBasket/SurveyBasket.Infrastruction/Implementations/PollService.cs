﻿

using Hangfire;

namespace SurveyBasket.Infrastruction.Implementations;
internal class PollService(
    ApplicationDbContext context,
    INotificationService notificationService
    ) : IPollServices
{

    private readonly ApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var polls = await _context.Polls
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);


        return Result.Success<IEnumerable<PollResponse>>(polls);
    }


    public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
       var polls = await _context.Polls
            .Where(p => p.IsPublished 
            && p.StartsAt <=DateOnly.FromDateTime(DateTime.UtcNow)
            && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .ProjectToType<PollResponse>()
            .ToListAsync(cancellationToken);
        return Result.Success<IEnumerable<PollResponse>>(polls);

    }
    public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        return poll is not null
            ? Result.Success(poll.Adapt<PollResponse>())
            : Result.Failure<PollResponse>(PollErrors.PollNotFound);

    }
       

    public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
    {
        var isExistingTitle = await _context.Polls.AnyAsync(p => p.Title == request.Title, cancellationToken);

        if (isExistingTitle)
          return  Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);


        var poll = request.Adapt<Poll>();

        await _context.Polls.AddAsync(poll,cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(poll.Adapt<PollResponse>());
    }

    public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
    {

        var isExistingTitle = await _context.Polls
                    .AnyAsync(p => p.Title == request.Title && p.Id != id, cancellationToken);

        if (isExistingTitle)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);


        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);

        if (currentPoll is null)
            return Result.Failure(PollErrors.PollNotFound);

        currentPoll.Title = request.Title;
        currentPoll.Summary = request.Summary;
        currentPoll.StartsAt = request.StartsAt;
        currentPoll.EndsAt = request.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }


    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id,cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        _context.Remove(poll);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleIsPublishedAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);

        if (poll.IsPublished && poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollSNotification(poll.Id));


        return Result.Success();
    }

   
}
