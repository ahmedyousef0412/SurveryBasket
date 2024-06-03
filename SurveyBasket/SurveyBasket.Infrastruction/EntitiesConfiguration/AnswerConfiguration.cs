
namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {

        //Don't repeat the same answer for the Question.
        builder.HasIndex(a => new { a.QuestionId, a.Content }).IsUnique();

        builder.Property(a => a.Content)
            .HasMaxLength(1000);

    }
}
