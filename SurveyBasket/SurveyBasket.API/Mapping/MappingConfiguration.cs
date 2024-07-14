

namespace SurveyBasket.API.Mapping;

public class MappingConfiguration: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        #region Poll Mapping

        config.NewConfig<Poll, PollResponse>()
            .Map(dest => dest.Notes, src => src.Summary)
            .TwoWays();

        #endregion

        #region Question Mapping
                         //Src            //Dest
        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers,
            src => src.Answers.Select(answer => new Answer { Content = answer }));



        #endregion

        #region Auth Mapping

        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);
        #endregion
    }
}
