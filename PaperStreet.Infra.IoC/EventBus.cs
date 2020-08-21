using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Infra.Bus;

namespace PaperStreet.Infra.IoC
{
    public static class RegisterEventBus
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Domain Bus
            services.AddSingleton<IEventBus, RabbitMqBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMqBus(sp.GetService<IMediator>(), scopeFactory);
            });
        }
    }
}