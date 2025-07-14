using Confluent.Kafka;

namespace Producer.Configurations;

public class KafkaProducerConfig
{
    public static ProducerConfig BuildProducerConfig(KafkaProducerProperties options)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            ClientId = options.ClientId,
            Acks = Enum.Parse<Acks>(options.Acks, true),
            // Retries = options.Retries,
            BatchSize = options.BatchSize,
            LingerMs = options.LingerMs,
            CompressionType = Enum.Parse<CompressionType>(options.CompressionType, true),
            RequestTimeoutMs = options.RequestTimeoutMs,
            // DeliveryTimeoutMs = options.DeliveryTimeoutMs,
            SecurityProtocol = Enum.Parse<SecurityProtocol>(options.SecurityProtocol, true),
            EnableIdempotence = options.EnableIdempotence,
            // MaxInFlightRequestsPerConnection = options.MaxInFlightRequestsPerConnection,
        };

        // SASL configuration
        if (!string.IsNullOrEmpty(options.SaslMechanism))
        {
            config.SaslMechanism = Enum.Parse<SaslMechanism>(options.SaslMechanism, true);
            config.SaslUsername = options.SaslUsername;
            config.SaslPassword = options.SaslPassword;
        }

        // SSL configuration
        if (!string.IsNullOrEmpty(options.SslCaLocation))
        {
            config.SslCaLocation = options.SslCaLocation;
        }

        // Additional configuration
        foreach (var kvp in options.AdditionalConfig)
        {
            config.Set(kvp.Key, kvp.Value);
        }

        return config;
    }
}