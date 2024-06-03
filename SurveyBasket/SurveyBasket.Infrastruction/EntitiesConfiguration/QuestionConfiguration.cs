


namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        //Don't repeat the same question in the same Poll.
        builder.HasIndex(q => new { q.PollId, q.Content }).IsUnique();

        builder.Property(q => q.Content)
            .HasMaxLength(1000);

       
    }
}
