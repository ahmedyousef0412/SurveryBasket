

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
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        builder.HasData(new ApplicationUser
        {

            Id = DefaultUsers.AdminId,
            FirstName = DefaultUsers.AdminFirstName,
            LastName = DefaultUsers.AdminLastName,
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!,DefaultUsers.AdminPassword)
        });
    }
}
