﻿

namespace SurveyBasket.Application.Services;
public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetAsync(string id);

    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken);

    Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);

    Task<Result> ToggleStatusAsync(string id);
    Task<Result> Unlock(string id);
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}

