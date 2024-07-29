


namespace SurveyBasket.Infrastruction.Implementations;
public class RoleService(RoleManager<ApplicationRole> roleManager,ApplicationDbContext  context ) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false,CancellationToken cancellation = default)
    {
        return await _roleManager.Roles
            .Where(r => !r.IsDefault
            && (!r.IsDeleted
            || (includeDisabled.HasValue && includeDisabled.Value)))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellation);
    }
    public async Task<Result<RoleDetailsResponse>> GetAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleDetailsResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(p => p.Value));

        return Result.Success(response);

    }
    public async Task<Result<RoleDetailsResponse>> AddAsync(RoleRequest request)
    {
        var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);

        if (roleIsExists)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.DuplicatedRole);

        var allowedPermissions = Permessions.GetAllPermesions();

        if(request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailsResponse>(RoleErrors.InvalidPermissions);

        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            var permissions = request.Permissions
                .Select(p => new IdentityRoleClaim<string>
                {
                    ClaimType = Permessions.Type,
                    ClaimValue = p,
                    RoleId = role.Id
                });

            await _context.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();

            var response = new RoleDetailsResponse(role.Id, role.Name, role.IsDeleted, request.Permissions);

            return Result.Success(response);
        }

        var error = result.Errors.First();
        return Result.Failure<RoleDetailsResponse>(new Error(error.Code,error.Description , StatusCodes.Status400BadRequest));
    }
    public async Task<Result> UpdateAsync(string id, RoleRequest request)
    {
        var roleExists = await _roleManager.Roles
            .AnyAsync(r => r.Name == request.Name && r.Id != id);

        if(roleExists)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.DuplicatedRole);

        var role = await _roleManager.FindByIdAsync(id);

        if (role is null)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

        var allowedPermissions = Permessions.GetAllPermesions();

        if(request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailsResponse>(RoleErrors.InvalidPermissions);

        //Update
        role.Name = request.Name;

        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded) 
        {
            var currentPermissions = await _context.RoleClaims
                  .Where(r => r.RoleId == id && r.ClaimType == Permessions.Type)
                  .Select(r => r.ClaimValue)
                  .ToListAsync();

            var newPermissions = request.Permissions.Except(currentPermissions)
                .Select(p => new IdentityRoleClaim<string>
                {
                    ClaimType = Permessions.Type,
                    ClaimValue = p,
                    RoleId = role.Id
   
                
                });


            var removedPermissions = currentPermissions.Except(request.Permissions);

            await _context.RoleClaims
                .Where(r => r.RoleId == id && removedPermissions.Contains(r.ClaimValue))
                .ExecuteDeleteAsync();


            await _context.AddRangeAsync(newPermissions);
            await _context.SaveChangesAsync();


            return Result.Success();

        }

        var error = result.Errors.First();

        return Result.Failure<RoleDetailsResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ToggleStatusAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role is null)
            return Result.Failure<RoleDetailsResponse>(RoleErrors.RoleNotFound);

        role.IsDeleted = !role.IsDeleted;

        await _roleManager.UpdateAsync(role);

        return Result.Success();
    }
}
