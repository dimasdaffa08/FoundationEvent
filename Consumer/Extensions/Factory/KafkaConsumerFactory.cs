using Base.Configurations;
using Consumer.Interfaces;
using Consumer.Interfaces.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Consumer.Extensions.Factory;

public class KafkaConsumerFactory
{
    private readonly KafkaProperties _options;
    private readonly IServiceProvider _provider;

    public KafkaConsumerFactory(KafkaProperties options, IServiceProvider provider)
    {
        _options = _options = _options = options ?? throw new ArgumentNullException(nameof(options));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public IKafkaConsumer CreateConsumer()
    {
        var logger = _provider.GetRequiredService<ILogger<KafkaConsumerImpl>>();
        return new KafkaConsumerImpl(_options, logger);
    }
}