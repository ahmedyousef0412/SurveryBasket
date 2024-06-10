﻿


namespace SurveyBasket.Application.Services;
public interface IPollServices
{
    Task<Result<IEnumerable<PollResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> ToggleIsPublishedAsync(int id, CancellationToken cancellationToken = default);
}
