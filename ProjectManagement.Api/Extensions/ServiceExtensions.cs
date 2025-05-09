﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectManagement.Domain.Entities.Attachment;
using ProjectManagement.Domain.Entities.Country;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Entities.MultilingualText;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Entities.User;
using ProjectManagement.Service.Interfaces.Attachment;
using ProjectManagement.Service.Interfaces.Country;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Log;
using ProjectManagement.Service.Interfaces.MultilingualText;
using ProjectManagement.Service.Interfaces.Request;
using ProjectManagement.Service.Interfaces.User;
using ProjectManagement.Service.Service.Attachment;
using ProjectManagement.Service.Service.Country;
using ProjectManagement.Service.Service.Log;
using ProjectManagement.Service.Service.MultilingualText;
using ProjectManagement.Service.Service.Repositories;
using ProjectManagement.Service.Service.Request;
using ProjectManagement.Service.Service.Requests;
using ProjectManagement.Service.Service.User;
using System.Reflection;
using System.Text;
using TrustyTalents.Service.Services.Background;
using TrustyTalents.Service.Services.Emails;

namespace ProjectManagement.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Attachment>, GenericRepository<Attachment>>();
            services.AddScoped<IGenericRepository<Country>, GenericRepository<Country>>();
            services.AddScoped<IGenericRepository<Logs>, GenericRepository<Logs>>();
            services.AddScoped<IGenericRepository<Request>, GenericRepository<Request>>();
            services.AddScoped<IGenericRepository<RequestStatus>, GenericRepository<RequestStatus>>();
            services.AddScoped<IGenericRepository<MultilingualText>, GenericRepository<MultilingualText>>();
            services.AddScoped<IGenericRepository<RequestHistory>, GenericRepository<RequestHistory>>();
            services.AddScoped<IGenericRepository<ProcessingStatus>, GenericRepository<ProcessingStatus>>();
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddDependencies();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IRequestStatusService, RequestStatusService>();
            services.AddScoped<IMultilingualTextInterface, MultilingualTextService>();
            services.AddScoped<IProcessingStatusService, ProcessingStatusService>();
            services.AddSingleton<IEmailInboxService, EmailInboxService>();
            services.AddHostedService<EmailSenderBackgroundService>();
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
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "project-management",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3249f3f0-8b1e-4ebb-a8ee-1e40b0e90034dasfdq-asd23da"))
                };
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        }
    }
}
