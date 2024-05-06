

using Microsoft.AspNetCore.Http;

namespace SurveyBasket.Infrastruction.Persistence;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options , IHttpContextAccessor httpContextAccessor)
    :IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Poll> Polls { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var entities = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entityEnrey in entities)
        {
            if (entityEnrey.State == EntityState.Added)
            {
                entityEnrey.Property(x => x.CreatedById).CurrentValue = currentUserId!;
            }
            else if (entityEnrey.State == EntityState.Modified)
            {
                entityEnrey.Property(x => x.UpdatedById).CurrentValue = currentUserId!;
                entityEnrey.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
