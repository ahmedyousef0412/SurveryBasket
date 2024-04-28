


namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        

        builder.Property(p => p.FirstName)
            .HasMaxLength(120);

        builder.Property(p => p.LastName)
           .HasMaxLength(100);
    }
}
