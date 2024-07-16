
using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Serilog;
using SurveyBasket.Application.Services.Notifications;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.SurveyBasketApiDependeciesService(builder.Configuration);

//Will read from apsettings
builder.Host.UseSerilog((context, configuration) =>

  configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{

    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
    ],
    DashboardTitle = "Survey Basket Dashboard",
    //IsReadOnlyFunc = (DashboardContext context) => true
});

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

RecurringJob.AddOrUpdate("SendNewPollSNotification", () => notificationService.SendNewPollSNotification(null), Cron.Daily);

app.UseCors();

app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.UseExceptionHandler();
app.Run();
