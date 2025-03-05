using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Middlewres;
using ProjectManagement.Infrastructure.Contexts;
using ProjectManagement.Service.Extencions;
using ProjectManagement.Service.Interfaces.Class;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Student;
using ProjectManagement.Service.Service.Class;
using ProjectManagement.Service.Service.Repositories;
using ProjectManagement.Service.Service.Student;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IClassService, ClassService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<ProjectManagementDB>(options =>
{
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ProjectManagement.Api"));
    options.EnableDetailedErrors();
});

builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<LowercaseDocumentFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ProjectManagementExceptionMiddlewares>();
app.UseAuthorization();

app.MapControllers();

app.Run();
