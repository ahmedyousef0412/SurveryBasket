global using Asp.Versioning;
global using Hangfire;
global using HangfireBasicAuthenticationFilter;
global using HealthChecks.UI.Client;
global using MailKit.Net.Smtp;
global using MailKit.Security;
global using Mapster;
global using MapsterMapper;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.RateLimiting;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using SurveyBasket.API.Configuration;
global using SurveyBasket.API.Extensions;
global using SurveyBasket.API.Helper;
global using SurveyBasket.API.Middleware;
global using SurveyBasket.Application.Services;
global using SurveyBasket.Application.Services.Notifications;
global using SurveyBasket.Contracts.Authentication;
global using SurveyBasket.Contracts.Common;
global using SurveyBasket.Contracts.Configurations;
global using SurveyBasket.Contracts.Polls;
global using SurveyBasket.Contracts.Questions;
global using SurveyBasket.Contracts.Roles;
global using SurveyBasket.Contracts.Users;
global using SurveyBasket.Contracts.Votes;
global using SurveyBasket.Domain.Abstractions;
global using SurveyBasket.Domain.Abstractions.Consts;
global using SurveyBasket.Domain.Entities;
global using SurveyBasket.Infrastruction.ConfigureServices;
global using SurveyBasket.Infrastruction.Implementations.Authentications.Filters;
global using SurveyBasket.Infrastruction.Persistence;
global using SurveyBasket.Infrastruction.Settings;
global using System.Reflection;
global using System.Security.Claims;
global using System.Threading.RateLimiting;
