

using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using SurveyBasket.API.Configuration;
using SurveyBasket.Contracts.Configurations;
using SurveyBasket.Infrastruction.ConfigureServices;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);



builder.Services.SurveyBasketApiDependeciesService();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
