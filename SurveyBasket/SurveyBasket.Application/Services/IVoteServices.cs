

using SurveyBasket.Contracts.Votes;

namespace SurveyBasket.Application.Services;
public interface IVoteServices
{
    Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default);
}
