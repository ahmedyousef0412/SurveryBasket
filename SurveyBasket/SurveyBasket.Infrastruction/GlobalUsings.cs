global using Microsoft.Extensions.DependencyInjection;
global using SurveyBasket.Application.Services;
global using SurveyBasket.Infrastruction.Implementations;
global using SurveyBasket.Domain.Entities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using SurveyBasket.Infrastruction.Persistence;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using System.Reflection;
global using SurveyBasket.Contracts.Authentication;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using SurveyBasket.Infrastruction.EntitiesConfiguration;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Configuration;
global using Microsoft.AspNetCore.Http;
global using SurveyBasket.Domain.Abstractions;
global using SurveyBasket.Domain.Errors;
global using System.Security.Cryptography;
global using Mapster;

global using SurveyBasket.Contracts.Polls;
global using SurveyBasket.Contracts.Results;
global using SurveyBasket.Application.Services.Caching;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.Extensions.Logging;
global using SurveyBasket.Infrastruction.Implementations.Caching;
global using MailKit.Net.Smtp;
global using MailKit.Security;
global using Microsoft.AspNetCore.Identity.UI.Services;
global using MimeKit;
global using SurveyBasket.Infrastruction.Settings;
global using SurveyBasket.Infrastruction.Helper;
global using SurveyBasket.Application.Services.Notifications;

global using Hangfire;
global using SurveyBasket.Infrastruction.Implementations.EmailSender;
global using SurveyBasket.Infrastruction.Implementations.Notification;
global using SurveyBasket.Contracts.Users;
global using SurveyBasket.Domain.Abstractions.Consts;
global using Microsoft.AspNetCore.Authorization;
global using SurveyBasket.Infrastruction.Implementations.Authentications.Filters;
global using SurveyBasket.Contracts.Roles;
global using System.Data;
global using SurveyBasket.Contracts.Answers;
global using SurveyBasket.Contracts.Common;
global using SurveyBasket.Contracts.Questions;
global using System.Linq.Dynamic.Core;



