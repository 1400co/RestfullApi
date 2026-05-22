using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SocialMedia.Infrastructure.Extentions;
using SocialMedia.Infrastructure.Filters;
using System;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

namespace SocialMedia.Api
{
    public class Startup
    {
        //Add test comment.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //read all automapper from assembly on infraestructure/Mappers
            services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                // .WithOrigins(origins)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    });
            });

            services.AddControllers(
                options => options.Filters.Add<GlobalExceptionFilter>()
                )
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                .ConfigureApiBehaviorOptions(options =>
                    {
                        //options.SuppressModelStateInvalidFilter = true;
                    });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue; // L�mite de tama�o de carga (en bytes). En este caso, sin l�mite.
                options.MultipartBodyLengthLimit = long.MaxValue; // L�mite de tama�o del cuerpo multipart. En este caso, sin l�mite.
                options.MultipartHeadersLengthLimit = int.MaxValue; // L�mite de tama�o de los encabezados multipart. En este caso, sin l�mite.
            });

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("AuthPolicy", config =>
                {
                    config.PermitLimit = 5;
                    config.Window = TimeSpan.FromMinutes(1);
                    config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    config.QueueLimit = 0;
                });
                options.RejectionStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status429TooManyRequests;
            });

            //Use extentions methods from infraestructure Extentions.
            services.AddOptions(Configuration);

            //services.AddDbContexts(Configuration); //SqlServer
            services.AddDbContextsPostgress(Configuration); //Postgress

            services.AddHangfire(Configuration);
            services.AddFluentValidationAutoValidation();
            services.AddServices();
            services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? Configuration["Authentication:SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };
            });

            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media Api V1");
                options.RoutePrefix = String.Empty;
            });

            app.UseCors("AllowAllOrigins");

            app.UseRateLimiter();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
