using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Producer.Configurations;
using Producer.Interfaces;
using Producer.Interfaces.Impl;

namespace Producer.Extensions.Factory;

public class KafkaProducerFactory
{
    private readonly KafkaProducerProperties _options;
    private readonly IServiceProvider _serviceProvider;

    public KafkaProducerFactory(KafkaProducerProperties options, IServiceProvider serviceProvider)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IKafkaProducer<TKey, TValue> CreateProducer<TKey, TValue>()
        where TValue : IMessage<TValue>
    {
        var config = _options;
        var logger = _serviceProvider.GetRequiredService<ILogger<KafkaProducerImpl<TKey, TValue>>>();
        
        return new KafkaProducerImpl<TKey, TValue>(config, logger);
    }
}