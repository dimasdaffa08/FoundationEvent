using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Producer.Configurations;
using Producer.Interfaces;
using Producer.Interfaces.Impl;

namespace Producer.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services,
        Action<KafkaProducerProperties> configureOptions)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
            
        if (configureOptions == null)
            throw new ArgumentNullException(nameof(configureOptions));

        var options = new KafkaProducerProperties();
        configureOptions(options);

        services.TryAddSingleton<IKafkaProducer<TKey, TValue>>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<KafkaProducerImpl<TKey, TValue>>>();
            return new KafkaProducerImpl<TKey, TValue>(options, logger);
        });

        return services;
    }
    
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(
        this IServiceCollection services,
        KafkaProducerProperties options)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
            
        if (options == null)
            throw new ArgumentNullException(nameof(options));

        services.TryAddSingleton<IKafkaProducer<TKey, TValue>>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<KafkaProducerImpl<TKey, TValue>>>();
            return new KafkaProducerImpl<TKey, TValue>(options, logger);
        });

        return services;
    }
}