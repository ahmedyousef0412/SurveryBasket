namespace SurveyBasket.API.Mapping;

public class MappingConfiguration: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        #region Poll Mapping

        config.NewConfig<Poll, PollResponse>()
            .Map(dest => dest.Notes, src => src.Description)
            .TwoWays();

        #endregion
    }
}
