using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Api.Extensions;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Service.Repositories;
using System.Text;
using System.Text.Json.Serialization;
using static ProjectManagement.Service.Service.Attachment.AttachmentService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
var configuration = builder.Configuration;

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(e => e.AddDefaultPolicy(e =>
            e.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod()
    // .AllowCredentials()
    )
);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ProjectManagementDB>(options =>
{
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ProjectManagement.Api"));
    options.EnableDetailedErrors();
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddAuthorization();

builder.Services.AddSignalR();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger).AddConsole();

builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddSwaggerService();
builder.Services.AddCustomServices();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Services.GetRequiredService<IWebHostEnvironment>();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

EnvironmentHelper.WebRootPath = app.Services.GetRequiredService<IWebHostEnvironment>()?.WebRootPath;
app.UseStaticFiles();
app.UseAuthentication();
app.UseHttpsRedirection();
//app.UseMiddleware<ProjectManagementExceptionMiddlewares>();
app.UseAuthorization();

app.MapControllers();

app.Run();
