using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Infra.Security;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Infra.Bus;
using PaperStreet.Logging.Application.EventHandlers.User;
using PaperStreet.Logging.Data.Context;

namespace PaperStreet.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Domain Bus
            services.AddSingleton<IEventBus, RabbitMqBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMqBus(sp.GetService<IMediator>(), scopeFactory);
            });
            
            // Subscriptions
            services.AddTransient<UserRegisteredEventHandler>();
            
            // Domain Events
            services.AddTransient<IEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();

            // Application Services
            services.AddTransient<IJwtGenerator, JwtGenerator>();

            // Data
            services.AddTransient<AuthenticationDbContext>();
            services.AddTransient<LoggingDbContext>();
        }
    }
}