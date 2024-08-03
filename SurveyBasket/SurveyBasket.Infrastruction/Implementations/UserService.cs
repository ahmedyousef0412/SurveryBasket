


namespace SurveyBasket.Infrastruction.Implementations;
internal class UserService(UserManager<ApplicationUser> userManager
    ,IRoleService roleService
    ,ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IRoleService _roleService = roleService;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await 
            (
                      from u in _context.Users
                      join
                      ur in _context.UserRoles on u.Id equals ur.UserId

                      join r in _context.Roles on ur.RoleId equals r.Id
                      into roles
                      where !roles.Any(x => x.Name == DefaultRoles.Member)
                      select new
                      {
                          u.Id,
                          u.FirstName,
                          u.LastName,
                          u.Email,
                          u.IsDisabled,
                          Roles = roles.Select(r => r.Name!).ToList()
                      }
            )
                .GroupBy
                (u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.IsDisabled
                }
                )
                .Select(u => new UserResponse
                (
                    u.Key.Id,
                    u.Key.FirstName,
                    u.Key.LastName,
                    u.Key.Email,
                    u.Key.IsDisabled,
                    u.SelectMany(u => u.Roles)
                ))
                .ToListAsync(cancellationToken);
    }


    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return Result.Failure<UserResponse>(UserError.UserNotFound);

        var userRoles = await _userManager.GetRolesAsync(user);


        var response = (user, userRoles).Adapt<UserResponse>();

        return Result.Success(response);    
    }
    

    public async Task<Result<UserResponse>>AddAsync(CreateUserRequest request , CancellationToken cancellationToken)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email,cancellationToken);

        if (emailIsExists)
            return Result.Failure<UserResponse>(UserError.DuplicatedEmail);

        var allowedRoles = await _roleService.GetAllAsync(cancellationToken : cancellationToken);

        if (request.Roles.Except(allowedRoles.Select(r => r.Name)).Any())
            return Result.Failure<UserResponse>(UserError.InvalidRoles);

        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {

            await _userManager.AddToRolesAsync(user, request.Roles);

            var response = (user,request.Roles).Adapt<UserResponse>();

            return Result.Success(response);
        }
        var error = result.Errors.First();

        return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }



    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email && u.Id != id, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserError.DuplicatedEmail);

        var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);


        if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
            return Result.Failure(UserError.InvalidRoles);

        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return Result.Failure(UserError.UserNotFound);

        user = request.Adapt(user);

        var result = await _userManager.UpdateAsync(user);


        if (result.Succeeded)
        {
            await _context.UserRoles
                .Where(ur => ur.UserId == id)
                .ExecuteDeleteAsync(cancellationToken);


            await _userManager.AddToRolesAsync(user, request.Roles);

            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<Result> ToggleStatusAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return Result.Failure(UserError.UserNotFound);

        user.IsDisabled = !user.IsDisabled;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<Result> Unlock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            return Result.Failure(UserError.UserNotFound);


        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Success(user);
    }



    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {

        await _userManager.Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setter =>

              setter

              .SetProperty(u => u.FirstName, request.FirstName)
              .SetProperty(u => u.LastName, request.LastName)
            );

        return Result.Success();
    }



    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user =  await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if(result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    
}
