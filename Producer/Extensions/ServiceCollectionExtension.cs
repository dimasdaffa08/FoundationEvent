using Base.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Producer.Extensions.Factory;

namespace Producer.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services, Action<KafkaProperties> configureOptions)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
            
        if (configureOptions == null)
            throw new ArgumentNullException(nameof(configureOptions));

        var options = new KafkaProperties();
        configureOptions(options);
        
        services.TryAddSingleton(options);
        services.TryAddSingleton<KafkaProducerFactory>();
        return services;
    }
    
    public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services, KafkaProperties options)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
            
        if (options == null)
            throw new ArgumentNullException(nameof(options));
        
        services.TryAddSingleton(options);
        services.TryAddSingleton<KafkaProducerFactory>();

        return services;
    }
}