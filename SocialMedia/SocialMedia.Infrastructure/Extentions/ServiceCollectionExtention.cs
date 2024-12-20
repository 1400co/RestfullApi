﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Options;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;
using System;
using Hangfire;
using System.IO;
using TransforSerPu.Core.Interfaces;
using Hangfire.PostgreSql;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Extentions
{
    public static class ServiceCollectionExtention
    {
        /// <summary>
        /// Use to change form Sqlserver o Postgres
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AddDbContextsSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            var engine = configuration["Database:Engine"];
            var connectionStringName = "MyConn"; // Asumiendo que quieres usar la misma cadena de conexión para ambos

            services.AddDbContext<SocialMediaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(connectionStringName)));

            return services;
        }

        /// <summary>
        /// Use to change form Sqlserver o Postgres
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IServiceCollection AddDbContextsPostgress(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringName = "MyConn";

            services.AddDbContext<SocialMediaContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString(connectionStringName))
            );

            return services;
        }


        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(configurations.GetConnectionString("MyConn")));
            //.UseSqlServerStorage(configurations.GetConnectionString("MyConn")));

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 1;
            }); //Comment if web app will only create jobs for another service 

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));
            services.Configure<AuthenticationOptions>(options => configuration.GetSection("Authentication").Bind(options));
            services.Configure<SmtpSettings>(options => configuration.GetSection("SmtpSettings").Bind(options));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserInRolesService, UserInRolesService>();
            services.AddTransient<IRolModuleService, RolModuleService>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        {
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);

                // Agregar la descripción del esquema de seguridad JWT
                doc.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                // Agregar el esquema de seguridad a los documentos Swagger
                doc.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }
    }
}
