﻿
using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.Application.Services;
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(string firstName, string lastName, string userName, string email, string password, CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
}
