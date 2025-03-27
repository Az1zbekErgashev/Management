using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Extensions;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Service.Repositories;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;
using Telegram.Bot;
using static ProjectManagement.Service.Service.Attachment.AttachmentService;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = environment
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var botToken = builder.Configuration["TelegramBot:Token"];
builder.Services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}); ;
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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
    )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<LowercaseDocumentFilter>();
});

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

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

var env = app.Services.GetRequiredService<IWebHostEnvironment>();
EnvironmentHelper.WebRootPath = env.WebRootPath;

app.UseStaticFiles();

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();
