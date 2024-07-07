
using Serilog;

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


app.UseCors();

app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

app.UseExceptionHandler();
app.Run();
