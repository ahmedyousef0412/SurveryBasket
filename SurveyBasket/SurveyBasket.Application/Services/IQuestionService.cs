﻿

namespace SurveyBasket.Application.Services;
public interface IQuestionService
{

    Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilter filter, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default);

    Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default);


    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default);

    Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default);
}
