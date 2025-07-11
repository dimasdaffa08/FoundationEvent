using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Producer.Configurations;
using Producer.Model;

namespace Producer.Interfaces.Impl;

public class KafkaProducerImpl<TKey, TValue> : IKafkaProducer<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducerImpl<TKey, TValue>> _logger;
    private readonly KafkaProducerProperties _options;
    private bool _disposed = false;
    
    public KafkaProducerImpl(KafkaProducerProperties options, ILogger<KafkaProducerImpl<TKey, TValue>> logger)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrEmpty(options.BootstrapServers))
        {
            throw new ArgumentException("BootstrapServers cannot be empty", nameof(options));
        }

        var config = KafkaProducerConfig.BuildProducerConfig(options);
        _producer = new ProducerBuilder<TKey, TValue>(config)
            .SetErrorHandler((_, e) => _logger.LogError("Kafka producer error: {Error}", e.Reason))
            .SetLogHandler((_, log) => 
            {
                var logLevel = log.Level switch
                {
                    SyslogLevel.Emergency or SyslogLevel.Alert or SyslogLevel.Critical or SyslogLevel.Error => LogLevel.Error,
                    SyslogLevel.Warning => LogLevel.Warning,
                    SyslogLevel.Notice or SyslogLevel.Info => LogLevel.Information,
                    SyslogLevel.Debug => LogLevel.Debug,
                    _ => LogLevel.Information
                };
                _logger.Log(logLevel, "Kafka log: {Message}", log.Message);
            })
            .Build();

        _logger.LogInformation("Kafka publisher initialized with servers: {Servers}", options.BootstrapServers);
    }
    
    public async Task<KafkaProducerResponse> SendAsync(string topic, TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        return await SendAsync(topic, key, value, null, cancellationToken);
    }
    
    public async Task<KafkaProducerResponse> SendAsync(string topic, TKey key, TValue value, IDictionary<string, string>? headers, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentException("Topic cannot be empty", nameof(topic));
            }

            try
            {
                // Convert headers to Kafka format
                Headers? kafkaHeaders = null;
                if (headers != null)
                {
                    kafkaHeaders = new Headers();
                    foreach (var header in headers)
                    {
                        kafkaHeaders.Add(header.Key, System.Text.Encoding.UTF8.GetBytes(header.Value));
                    }
                }

                var message = new Message<TKey, TValue>
                {
                    Key = key,
                    Value = value,
                    Headers = kafkaHeaders
                };

                _logger.LogDebug("Sending message to topic: {Topic}", topic);

                var deliveryResult = await _producer.ProduceAsync(topic, message, cancellationToken);

                _logger.LogDebug("Message sent successfully to {Topic}[{Partition}]@{Offset}", 
                    deliveryResult.Topic, deliveryResult.Partition, deliveryResult.Offset);

                return new KafkaProducerResponse
                {
                    Topic = deliveryResult.Topic,
                    Partition = deliveryResult.Partition,
                    Offset = deliveryResult.Offset,
                    Timestamp = deliveryResult.Timestamp.UtcDateTime,
                    IsSuccess = true,
                    Headers = headers
                };
            }
            catch (ProduceException<TKey, TValue> ex)
            {
                _logger.LogError(ex, "Failed to send message to topic: {Topic}", topic);
                
                return new KafkaProducerResponse
                {
                    Topic = topic,
                    IsSuccess = false,
                    Error = ex.Error.Reason
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while sending message to topic: {Topic}", topic);
                
                return new KafkaProducerResponse
                {
                    Topic = topic,
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }
    
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(KafkaProducerImpl<TKey, TValue>));
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _logger.LogInformation("Disposing Kafka Producer");
            _producer?.Dispose();
            _disposed = true;
        }
    }
}