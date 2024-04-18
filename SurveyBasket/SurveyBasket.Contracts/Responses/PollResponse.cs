
namespace SurveyBasket.Contracts.Responses;
public record PollResponse(
    int Id,
    string Title, 
    string Notes,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt);
