using Base.Configurations;
using Confluent.Kafka;
using Consumer.Configurations;
using Microsoft.Extensions.Logging;

namespace Consumer.Interfaces.Impl;

public class KafkaConsumerImpl : IKafkaConsumer
{
    private readonly IConsumer<string, string> _consumer;
    private readonly ILogger<KafkaConsumerImpl> _logger;
    private readonly KafkaProperties _options;

    public KafkaConsumerImpl(KafkaProperties options, ILogger<KafkaConsumerImpl> logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        if (string.IsNullOrEmpty(options.BootstrapServers))
        {
            throw new ArgumentException("BootstrapServers cannot be empty", nameof(options));
        }

        var config = KafkaConsumerConfig.BuildConsumerConfig(options);

        var builder = new ConsumerBuilder<string, string>(config);
        
        builder.SetErrorHandler((_, e) => _logger.LogError("Kafka consumer error: {Error}", e.Reason));
        builder.SetLogHandler((_, log) =>
        {
            var logLevel = log.Level switch
            {
                SyslogLevel.Emergency or SyslogLevel.Alert or SyslogLevel.Critical or SyslogLevel.Error => LogLevel
                    .Error,
                SyslogLevel.Warning => LogLevel.Warning,
                SyslogLevel.Notice or SyslogLevel.Info => LogLevel.Information,
                SyslogLevel.Debug => LogLevel.Debug,
                _ => LogLevel.Information
            };
            _logger.Log(logLevel, "Kafka log: {Message}", log.Message);
        });
        builder.SetValueDeserializer(Deserializers.Utf8);

        _consumer = builder.Build();
        
        _logger.LogInformation("Kafka consumer initialized with servers: {Servers}", options.BootstrapServers);
    }

    public async Task ListenAsync(string topic, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(topic))
        {
            throw new ArgumentException("Topic cannot be empty", nameof(topic));
        }

        if (string.IsNullOrEmpty(_options.GroupId))
        {
            throw new ArgumentException("GroupId cannot be empty", nameof(_options.GroupId));
        }

        _logger.LogInformation("Kafka consumer started. Listening to topic: {Topic}", topic);
        
        _consumer.Subscribe(topic);
        
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(cancellationToken);
                    
                    var headers = result.Message.Headers
                        .ToDictionary(
                            h => h.Key,
                            h => h.GetValueBytes() != null ? System.Text.Encoding.UTF8.GetString(h.GetValueBytes()) : null
                        );
                    
                    foreach (var header in headers)
                    {
                        _logger.LogInformation("Header: {Key} = {Value}", header.Key, header.Value);
                    }
                    
                    _logger.LogInformation("Message consumed: {Value}", result.Message.Value);

                    await Task.Yield();
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Consume error: {Reason}", ex.Error.Reason);
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation("Kafka consumer stopped by cancellation.");
        }
        finally
        {
            _consumer.Close();
        }
    }
}