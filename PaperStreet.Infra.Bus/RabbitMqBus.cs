using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Commands;
using PaperStreet.Domain.Core.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaperStreet.Infra.Bus
{
    public class RabbitMqBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }
        
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var eventName = @event.GetType().Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
                
            channel.BasicPublish("", eventName, null, body);
        }

        public void Subscribe<T, TH>() 
            where T : Event 
            where TH : IEventHandler
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} is already registered for '{eventName}'", nameof(handlerType));
            }
            
            _handlers[eventName].Add(handlerType);

            StartBasicConsumeMessage<T>();
        }

        private void StartBasicConsumeMessage<T>() where T : Event
        {
            var factory = new ConnectionFactory() {HostName = "localhost", DispatchConsumersAsync = true};

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consume_Received_Message;
            
            channel.BasicConsume(eventName, true, consumer);
        }

        private async Task Consume_Received_Message(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch
            {
                throw new Exception($"Problem consuming event {eventName}");
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var subscriptions = _handlers[eventName];
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ServiceProvider.GetService(subscription);
                    if (handler == null) continue;
                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType!);
                        
                    // Create concrete EventHandler for EventType
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    
                    // Fire Handle method for EventHandler
                    await (Task) concreteType.GetMethod("Handle").Invoke(handler, new object[] {@event});
                }
            }
        }
    }
}