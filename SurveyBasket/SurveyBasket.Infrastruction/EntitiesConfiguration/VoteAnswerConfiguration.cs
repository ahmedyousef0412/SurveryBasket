
namespace SurveyBasket.Infrastruction.EntitiesConfiguration;


internal class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(p => new { p.QuestionId, p.VoteId }).IsUnique();

    }
}
