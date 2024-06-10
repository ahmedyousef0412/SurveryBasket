

namespace SurveyBasket.Domain.Errors;
public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new("Question.NotFound", "No question was found with the given ID", StatusCodes.Status400BadRequest);


    public static readonly Error DuplicatedQuestionContent =
       new("Question.DuplicatedQuestionContent", "Question with this content already exist", StatusCodes.Status409Conflict);



}
