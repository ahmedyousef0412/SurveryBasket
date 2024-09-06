

namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.OwnsMany(e => e.RefreshTokens)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");


        builder.Property(p => p.FirstName)
            .HasMaxLength(120);

        builder.Property(p => p.LastName)
           .HasMaxLength(100);

        //Default Data

        builder.HasData(new ApplicationUser
        {

            Id = DefaultUsers.Admin.Id,
            FirstName = DefaultUsers.Admin.FirstName,
            LastName = DefaultUsers.Admin.LastName,
            UserName = DefaultUsers.Admin.Email,
            NormalizedUserName = DefaultUsers.Admin.Email.ToUpper(),
            Email = DefaultUsers.Admin.Email,
            NormalizedEmail = DefaultUsers.Admin.Email.ToUpper(),
            SecurityStamp = DefaultUsers.Admin.SecurityStamp,
            ConcurrencyStamp = DefaultUsers.Admin.ConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = DefaultUsers.Admin.PasswordHash
        });
    }
}
