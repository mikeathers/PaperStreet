using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaperStreet.Authentication.Data.Context;
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
            
            // Domain Banking Commands
            //services.AddTransient<IRequestHandler<CreateTransferFundsCommand, bool>, TransferFundsCommandHandler>();
            
            // Application Services
            //services.AddTransient<IAccountService, AccountService>();
            //services.AddTransient<ITransferService, TransferService>();
            
            // Data
            //services.AddTransient<IAccountRepository, AccountRepository>();
            //services.AddTransient<ITransferRepository, TransferRepository>();
            //services.AddTransient<BankingDbContext>();
            services.AddTransient<AuthenticationDbContext>();
        }
    }
}