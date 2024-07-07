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


