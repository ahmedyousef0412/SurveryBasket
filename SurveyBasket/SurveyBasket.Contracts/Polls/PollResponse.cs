namespace SurveyBasket.Contracts.Polls;
public record PollResponse(
    int Id,
    string Title,
    string Notes,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
);
public record PollResponseV2(
    int Id,
    string Title,
    string Notes,
    DateOnly StartsAt,
    DateOnly EndsAt
);
