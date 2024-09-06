


namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(p => new { p.PollId, p.UserId }).IsUnique();

    }
}
