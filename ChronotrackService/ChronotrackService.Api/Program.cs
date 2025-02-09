using ChronotrackService.Application.Models;
using ChronotrackService.Application;
using Microsoft.OpenApi.Models;
using ChronotrackService.Application.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var appSettings = new AppSettings(builder.Configuration);

builder.Services.AddServices(appSettings);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Chronotrack API",
        Version = "v1",
        Description = "Chronotrack API"
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chronotrack API");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
