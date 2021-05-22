﻿using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace KernX.EventBus.RabbitMQ
{
    public sealed class RabbitMQEventBus : IEventBus, IDisposable
    {
        private readonly ILogger<RabbitMQEventBus> _logger;

        private readonly IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private IModel _consumerChannel;

        public RabbitMQEventBus(ILogger<RabbitMQEventBus> logger, RabbitMQSettings settings)
        {
            _logger = logger;

            var connectionString =
                $"amqp://{settings.Username}:{settings.Password}@{settings.Host}:{settings.Port}/{settings.VHost}";
            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString),
                DispatchConsumersAsync = true
            };

            _connection = _connectionFactory.CreateConnection();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void Publish<T>(string exchange, string appId, T @event) where T : IEvent
        {
            using var channel = _connection.CreateModel();

            var properties = GetMessageProperties(channel, appId);
            var body = Encoding.UTF8.GetBytes(@event.Stringify());

            channel.BasicPublish(exchange, string.Empty, properties, body);
            _logger.LogInformation($"Sent Message: {body}");
        }

        public void Subscribe<T>(string queueName, Func<EventHeaders, T, Task> callback)
        {
            _consumerChannel = _connection.CreateModel();

            _consumerChannel.BasicQos(0, 1, false);
            _consumerChannel.CallbackException += ReceivedCallbackException;

            var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
            consumer.Received += async (_, eventArgs) => { await ReceivedCallback(eventArgs, callback); };

            _consumerChannel.BasicConsume(queueName, false, consumer);
            _logger.LogInformation("Press [enter] to exit.");
            Console.ReadLine();
        }

        #region Callbacks

        private void ReceivedCallbackException(object sender, CallbackExceptionEventArgs eventArgs)
        {
            _logger.LogError(eventArgs.Exception.ToString());
        }

        private async Task ReceivedCallback<T>(BasicDeliverEventArgs eventArgs, Func<EventHeaders, T, Task> callback)
        {
            var eventHeaders = EventHeaders.Create(eventArgs.BasicProperties.Headers);
            var message = new Message(eventArgs.Body.Span, eventHeaders);

            _logger.LogInformation($"[x] {message.ToString()}");

            var parsedMessage = await message.ParseMessage<T>();

            await callback(eventHeaders, parsedMessage);

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, false);
        }

        #endregion

        #region Helpers

        private static IBasicProperties GetMessageProperties(IModel channel, string appId)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = EventHeaders.ToDictionary(appId);
            return properties;
        }

        #endregion
    }
}