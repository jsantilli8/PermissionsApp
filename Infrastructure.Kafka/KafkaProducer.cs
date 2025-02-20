using System;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Interfaces;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly string? _topic;
        private readonly ILoggerService _logger;

        public KafkaProducer(IConfiguration configuration, ILoggerService logger)
        {
            _logger = logger;
            _topic = configuration["Kafka:Topic"];

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AllowAutoCreateTopics = true
            };

            using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = config.BootstrapServers }).Build())
            {
                try
                {
                    var topics = adminClient.GetMetadata(TimeSpan.FromSeconds(5)).Topics.Select(t => t.Topic).ToList();

                    if (!topics.Contains(_topic))
                    {
                        CreateTopic(adminClient, _topic).Wait();
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }


            _producer = new ProducerBuilder<string, string>(config).Build();
        }
        private async Task CreateTopic(IAdminClient adminClient, string topic)
        {
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification>
            {
                new TopicSpecification { Name = topic, NumPartitions = 1, ReplicationFactor = 1 }
            });

            }
            catch (CreateTopicsException e)
            {
                throw new Exception(e.Results[0].Error.Reason);
            }
        }
        public async Task SendOperationAsync(string operationType)
        {
            var message = new
            {
                Id = Guid.NewGuid(),
                Operation = operationType,
                Timestamp = DateTime.UtcNow
            };

            var serializedMessage = JsonSerializer.Serialize(message);

            try
            {
                await _producer.ProduceAsync(_topic, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = serializedMessage
                });

                _logger.LogInformation($"Kafka Message Sent: {serializedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Message sent Error Kafka: {ex.Message}");
            }
        }
    }
}
