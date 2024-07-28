
namespace SurveyBasket.Infrastruction.EntitiesConfiguration;
public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        var permissions = Permessions.GetAllPermesions();
        var adminClaims = new List<IdentityRoleClaim<string>>();

        for (int i = 0; i < permissions.Count; i++)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permessions.Type,
                ClaimValue = permissions[i],
                RoleId = DefaultRoles.AdminRoleId
            });
        }

        builder.HasData(adminClaims);
    }
}
