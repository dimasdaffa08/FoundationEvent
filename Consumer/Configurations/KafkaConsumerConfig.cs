using Base.Configurations;
using Confluent.Kafka;

namespace Consumer.Configurations;

public class KafkaConsumerConfig
{
    public static ConsumerConfig BuildConsumerConfig(KafkaProperties options)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = options.BootstrapServers,
            GroupId = options.GroupId,
            EnableAutoCommit = options.EnableAutoCommit,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(options.AutoOffsetReset, true),
            SessionTimeoutMs = options.SessionTimeoutMs,
            ClientId = options.ClientId,
            SecurityProtocol = Enum.Parse<SecurityProtocol>(options.SecurityProtocol, true),
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