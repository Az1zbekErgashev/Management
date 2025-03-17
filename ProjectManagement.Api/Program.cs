using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Extensions;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Service.Repositories;
using Serilog;
using static ProjectManagement.Service.Service.Attachment.AttachmentService;

var builder = WebApplication.CreateBuilder(args);

// ¬°¬á¬â¬Ö¬Õ¬Ö¬Ý¬ñ¬Ö¬Þ ¬ä¬Ö¬Ü¬å¬ë¬å¬ð ¬ã¬â¬Ö¬Õ¬å
var environment = builder.Environment.EnvironmentName;

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();


// ¬¯¬Ñ¬ã¬ä¬â¬à¬Û¬Ü¬Ñ ForwardedHeadersOptions ¬Õ¬Ý¬ñ ¬â¬Ñ¬Ò¬à¬ä¬í ¬ã ¬á¬â¬à¬Ü¬ã¬Ú-¬ã¬Ö¬â¬Ó¬Ö¬â¬Ñ¬Þ¬Ú
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// ¬°¬ä¬Ü¬Ý¬ð¬é¬Ñ¬Ö¬Þ ¬Ñ¬Ó¬ä¬à¬Þ¬Ñ¬ä¬Ú¬é¬Ö¬ã¬Ü¬à¬Ö ¬Ó¬Ñ¬Ý¬Ú¬Õ¬Ñ¬è¬Ú¬ð ¬Þ¬à¬Õ¬Ö¬Ý¬Ú
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// ¬²¬Ñ¬Ù¬â¬Ö¬ê¬Ñ¬Ö¬Þ CORS ¬Õ¬Ý¬ñ ¬Ó¬ã¬Ö¬ç ¬Ù¬Ñ¬á¬â¬à¬ã¬à¬Ó
builder.Services.AddCors(e => e.AddDefaultPolicy(e =>
            e.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod()
    // .AllowCredentials()
    )
);

// ¬¬¬à¬ß¬æ¬Ú¬Ô¬å¬â¬Ñ¬è¬Ú¬ñ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<LowercaseDocumentFilter>();
});

// ¬¯¬Ñ¬ã¬ä¬â¬à¬Û¬Ü¬Ñ ¬á¬à¬Õ¬Ü¬Ý¬ð¬é¬Ö¬ß¬Ú¬ñ ¬Ü ¬Ò¬Ñ¬Ù¬Ö ¬Õ¬Ñ¬ß¬ß¬í¬ç PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ProjectManagementDB>(options =>
{
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ProjectManagement.Api"));
    options.EnableDetailedErrors();
});

// ¬£¬Ü¬Ý¬ð¬é¬Ñ¬Ö¬Þ ¬á¬à¬Õ¬Õ¬Ö¬â¬Ø¬Ü¬å LegacyTimestampBehavior ¬Õ¬Ý¬ñ Npgsql
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ¬¥¬à¬Ò¬Ñ¬Ó¬Ý¬ñ¬Ö¬Þ ¬á¬à¬Õ¬Õ¬Ö¬â¬Ø¬Ü¬å ¬Ñ¬Ó¬ä¬à¬â¬Ú¬Ù¬Ñ¬è¬Ú¬Ú
builder.Services.AddAuthorization();

builder.Services.AddSignalR();

// ¬¯¬Ñ¬ã¬ä¬â¬à¬Û¬Ü¬Ñ ¬Ý¬à¬Ô¬Ú¬â¬à¬Ó¬Ñ¬ß¬Ú¬ñ ¬ã ¬Ú¬ã¬á¬à¬Ý¬î¬Ù¬à¬Ó¬Ñ¬ß¬Ú¬Ö¬Þ Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger).AddConsole();

// ¬±¬à¬Õ¬Ü¬Ý¬ð¬é¬Ö¬ß¬Ú¬Ö ¬Ó¬ã¬Ö¬ç ¬ã¬Ö¬â¬Ó¬Ú¬ã¬à¬Ó ¬Ú ¬Ü¬à¬ß¬æ¬Ú¬Ô¬å¬â¬Ñ¬è¬Ú¬Û
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddSwaggerService();
builder.Services.AddCustomServices();
builder.Services.AddHttpContextAccessor();

// ¬³¬à¬Ù¬Õ¬Ñ¬Ö¬Þ ¬á¬â¬Ú¬Ý¬à¬Ø¬Ö¬ß¬Ú¬Ö
var app = builder.Build();

// ¬µ¬Ò¬Ö¬Õ¬Ú¬Þ¬ã¬ñ, ¬é¬ä¬à ¬ß¬Ñ¬ã¬ä¬â¬à¬Û¬Ü¬Ú Swagger ¬ä¬à¬Ý¬î¬Ü¬à ¬Õ¬Ý¬ñ Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ¬±¬à¬Ý¬å¬é¬Ñ¬Ö¬Þ ¬ä¬Ö¬Ü¬å¬ë¬å¬ð ¬ã¬â¬Ö¬Õ¬å
var env = app.Environment.EnvironmentName;

// ¬£¬Ü¬Ý¬ð¬é¬Ñ¬Ö¬Þ CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// ¬¯¬Ñ¬ã¬ä¬â¬à¬Û¬Ü¬Ñ WebRootPath
EnvironmentHelper.WebRootPath = app.Services.GetRequiredService<IWebHostEnvironment>()?.WebRootPath;

// ¬³¬ä¬Ñ¬ä¬Ú¬é¬Ö¬ã¬Ü¬Ú¬Ö ¬æ¬Ñ¬Û¬Ý¬í
app.UseStaticFiles();

// ¬¡¬å¬ä¬Ö¬ß¬ä¬Ú¬æ¬Ú¬Ü¬Ñ¬è¬Ú¬ñ ¬Ú ¬â¬Ö¬Õ¬Ú¬â¬Ö¬Ü¬ä ¬ß¬Ñ HTTPS
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();

// ¬®¬Ñ¬â¬ê¬â¬å¬ä¬í
app.MapControllers();

app.Run();
