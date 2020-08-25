using System;
using System.Text;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Infra.IoC;
using PaperStreet.Logging.Api.Middleware;
using PaperStreet.Logging.Application.EventHandlers;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Application.Queries;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Data.Repository;

namespace PaperStreet.Logging.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LoggingDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("LoggingDbConnection"));
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Logging Microservice",
                    Version = "v1"
                });
            });
            
            services.AddMediatR(typeof(AllAuthenticationLogsQuery).Assembly);

            RegisterIoCServices(services);
            AddJwtAuthentication(services, Configuration);

            services.AddControllers().AddFluentValidation(cfg => 
            {
                cfg.RegisterValidatorsFromAssemblyContaining<AllAuthenticationLogsQuery>();
            });
        }
        
        private static void AddJwtAuthentication(IServiceCollection services, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => 
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        private static void RegisterIoCServices(IServiceCollection services)
        {
            RegisterEventBus.RegisterServices(services);
            
            // Subscriptions
            services.AddTransient<AuthenticationLogEventHandler>();
            services.AddTransient<ErrorLogEventHandler>();
            
            // Domain Events
            services.AddTransient<IEventHandler<AuthenticationLogEvent>, AuthenticationLogEventHandler>();
            services.AddTransient<IEventHandler<ErrorLogEvent>, ErrorLogEventHandler>(); 
            
            // Data
            services.AddTransient<IAuthenticationLogRepository, AuthenticationLogRepository>();
            services.AddTransient<IErrorLogRepository, ErrorLogRepository>();
            services.AddTransient<LoggingDbContext>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Logging Microservice v1");
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            ConfigureEventBus(app);
        }
        
        private static void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<AuthenticationLogEvent, AuthenticationLogEventHandler>();
            eventBus.Subscribe<ErrorLogEvent, ErrorLogEventHandler>();
        }
    }
}