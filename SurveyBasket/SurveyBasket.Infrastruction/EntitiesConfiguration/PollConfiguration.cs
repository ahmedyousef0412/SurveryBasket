


namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal class PollConfiguration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasIndex(p => p.Title).IsUnique();

        builder.Property(p => p.Title)
            .HasMaxLength(120);

        builder.Property(p => p.Summary)
           .HasMaxLength(1000);
    }
}
