using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using ProjectManagement.Domain.Entities.Teams;
using ProjectManagement.Domain.Entities.User;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.User;
using ProjectManagement.Service.Service.Repositories;
using ProjectManagement.Service.Service.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProjectManagement.Service.Interfaces.Attachment;
using ProjectManagement.Service.Service.Attachment;
using ProjectManagement.Domain.Entities.Attachment;
using ProjectManagement.Domain.Entities.Country;
using ProjectManagement.Service.Interfaces.Country;
using ProjectManagement.Service.Service.Country;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Service.Interfaces.Log;
using ProjectManagement.Service.Service.Log;
using ProjectManagement.Domain.Entities.Partners;

namespace ProjectManagement.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<TeamMember>, GenericRepository<TeamMember>>();
            services.AddScoped<IGenericRepository<Team>, GenericRepository<Team>>();
            services.AddScoped<IGenericRepository<Attachment>, GenericRepository<Attachment>>();
            services.AddScoped<IGenericRepository<Country>, GenericRepository<Country>>();
            services.AddScoped<IGenericRepository<Logs>, GenericRepository<Logs>>();
            services.AddScoped<IGenericRepository<Partners>, GenericRepository<Partners>>();
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddDependencies();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ILogService, LogService>();
        }
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(p =>
            {
                p.ResolveConflictingActions(ad => ad.First());
                p.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                p.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }      
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        }
    }
}
