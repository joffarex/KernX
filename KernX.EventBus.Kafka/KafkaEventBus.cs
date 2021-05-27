using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace KernX.EventBus.Kafka
{
    public sealed class KafkaEventBus : IEventBus, IDisposable
    {
        private readonly ILogger<KafkaEventBus> _logger;

        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Null, string> _consumer;

        public KafkaEventBus(ILogger<KafkaEventBus> logger, KafkaSettings settings)
        {
            _logger = logger;
            var connectionString = $"{settings.Host}:${settings.Port}";

            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = connectionString,
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            var consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = connectionString
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }


        public async Task Publish<T>(string topic, string appId, T @event) where T : IEvent
        {
            Dictionary<string, object> headers = EventHeaders.ToDictionary(appId);

            var kafkaHeaders = new Headers();
            foreach ((string key, object value) in headers)
            {
                kafkaHeaders.Add(key, Encoding.UTF8.GetBytes(value.ToString() ?? string.Empty));
            }
            
            await _producer.ProduceAsync(topic, new Message<Null, string>()
            {
                Value = @event.Stringify(),
                Headers = kafkaHeaders
            });

            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public async Task Subscribe<T>(string queue, Func<EventHeaders, T, Task> callback)
        {
            
        }

        public void Dispose()
        {
            _producer.Dispose();
            _consumer.Dispose();
        }
    }
}