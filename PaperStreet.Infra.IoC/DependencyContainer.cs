using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Authentication.Infra.Security;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Infra.Bus;

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
            //services.AddTransient<TransferEventHandler>();
            
            // Domain Events
            //services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();
            
            // Domain User Commands
            //services.AddTransient<IRequestHandler<Register.Command, User>, Register>();
            
            // Application Services
            services.AddTransient<IJwtGenerator, JwtGenerator>();
            
            
            // Data
            services.AddTransient<AuthenticationDbContext>();
        }
    }
}