


namespace SurveyBasket.API.Mapping;

public class MappingConfiguration : IRegister
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

        #region User Mapping


        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
            .Map(dest => dest, src => src.user)
            .Map(dest => dest.Roles, src => src.roles);


        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.EmailConfirmed, src => true);



        config.NewConfig<UpdateUserRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Email)
           .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper());
        #endregion
    }
}
