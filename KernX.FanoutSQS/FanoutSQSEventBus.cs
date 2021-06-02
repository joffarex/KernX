using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using KernX.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MessageAttributeValue = Amazon.SimpleNotificationService.Model.MessageAttributeValue;

namespace KernX.FanoutSQS
{
    public sealed class FanoutSQSEventBus : IEventBus, IDisposable
    {
        private readonly ILogger<FanoutSQSEventBus> _logger;
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly IAmazonSQS _sqsClient;

        public FanoutSQSEventBus(ILogger<FanoutSQSEventBus> logger, IConfiguration configuration)
        {
            _logger = logger;

            AWSOptions awsOptions = configuration.GetAWSOptions();

            _snsClient = awsOptions.CreateServiceClient<IAmazonSimpleNotificationService>();
            _sqsClient = awsOptions.CreateServiceClient<IAmazonSQS>();
        }

        public void Dispose()
        {
            _snsClient.Dispose();
            _sqsClient.Dispose();
        }

        public async Task Publish<T>(string topic, string appId, T @event) where T : IEvent
        {
            var request = new PublishRequest
            {
                Message = @event.Stringify(), TopicArn = topic, MessageAttributes = GetMessageAttributes(appId)
            };

            PublishResponse response = await _snsClient.PublishAsync(request);
            _logger.LogInformation($"Sent message SQS ID: {response.MessageId}");
        }

        public async Task Subscribe<T>(string queue, Func<EventHeaders, T, Task> callback)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queue, MessageAttributeNames = EventHeaders.GetNames
            };

            ReceiveMessageResponse response = await _sqsClient.ReceiveMessageAsync(request);
            Message message = response.Messages[0];

            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(message.Body));
            var body = await JsonSerializer.DeserializeAsync<T>(stream);
            await callback(EventHeaders.Create(message.Attributes), body);
        }

        private static Dictionary<string, MessageAttributeValue> GetMessageAttributes(string appId)
        {
            return EventHeaders.ToDictionary(appId)
                .ToDictionary(x => x.Key, x =>
                {
                    var attribute = new MessageAttributeValue
                    {
                        StringValue = (string) x.Value,
                        DataType = "String"
                    };

                    return attribute;
                });
        }
    }
}