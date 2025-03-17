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

// �����֬լ֬ݬ�֬� ��֬ܬ���� ���֬լ�
var environment = builder.Environment.EnvironmentName;

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();


// ���Ѭ����۬ܬ� ForwardedHeadersOptions �լݬ� ��ѬҬ��� �� ����ܬ��-��֬�Ӭ֬�Ѭެ�
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// ����ܬݬ��Ѭ֬� �ѬӬ��ެѬ�ڬ�֬�ܬ�� �ӬѬݬڬլѬ�ڬ� �ެ�լ֬ݬ�
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// ���Ѭ٬�֬�Ѭ֬� CORS �լݬ� �Ӭ�֬� �٬Ѭ������
builder.Services.AddCors(e => e.AddDefaultPolicy(e =>
            e.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod()
    // .AllowCredentials()
    )
);

// ����߬�ڬԬ��Ѭ�ڬ� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<LowercaseDocumentFilter>();
});

// ���Ѭ����۬ܬ� ���լܬݬ��֬߬ڬ� �� �ҬѬ٬� �լѬ߬߬�� PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ProjectManagementDB>(options =>
{
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ProjectManagement.Api"));
    options.EnableDetailedErrors();
});

// ���ܬݬ��Ѭ֬� ���լլ֬�جܬ� LegacyTimestampBehavior �լݬ� Npgsql
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ����ҬѬӬݬ�֬� ���լլ֬�جܬ� �ѬӬ���ڬ٬Ѭ�ڬ�
builder.Services.AddAuthorization();

builder.Services.AddSignalR();

// ���Ѭ����۬ܬ� �ݬ�Ԭڬ��ӬѬ߬ڬ� �� �ڬ���ݬ�٬�ӬѬ߬ڬ֬� Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger).AddConsole();

// ����լܬݬ��֬߬ڬ� �Ӭ�֬� ��֬�Ӭڬ��� �� �ܬ�߬�ڬԬ��Ѭ�ڬ�
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddSwaggerService();
builder.Services.AddCustomServices();
builder.Services.AddHttpContextAccessor();

// ����٬լѬ֬� ���ڬݬ�ج֬߬ڬ�
var app = builder.Build();

// ���Ҭ֬լڬެ��, ���� �߬Ѭ����۬ܬ� Swagger ���ݬ�ܬ� �լݬ� Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ����ݬ��Ѭ֬� ��֬ܬ���� ���֬լ�
var env = app.Environment.EnvironmentName;

// ���ܬݬ��Ѭ֬� CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// ���Ѭ����۬ܬ� WebRootPath
EnvironmentHelper.WebRootPath = app.Services.GetRequiredService<IWebHostEnvironment>()?.WebRootPath;

// ����Ѭ�ڬ�֬�ܬڬ� ��Ѭ۬ݬ�
app.UseStaticFiles();

// �����֬߬�ڬ�ڬܬѬ�ڬ� �� ��֬լڬ�֬ܬ� �߬� HTTPS
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();

// ���Ѭ������
app.MapControllers();

app.Run();
