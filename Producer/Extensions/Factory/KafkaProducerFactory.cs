using Base.Configurations;
using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Producer.Interfaces;
using Producer.Interfaces.Impl;

namespace Producer.Extensions.Factory;

public class KafkaProducerFactory
{
    private readonly KafkaProperties _options;
    private readonly IServiceProvider _serviceProvider;

    public KafkaProducerFactory(KafkaProperties options, IServiceProvider serviceProvider)
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