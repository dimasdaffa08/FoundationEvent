using Base.Configurations;
using Consumer.Extensions.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Consumer.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, Action<KafkaProperties> configureOptions)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

        var options = new KafkaProperties();
        configureOptions(options);

        services.TryAddSingleton(options);
        services.TryAddSingleton<KafkaConsumerFactory>();

        return services;
    }

    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, KafkaProperties options)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (options == null) throw new ArgumentNullException(nameof(options));

        services.TryAddSingleton(options);
        services.TryAddSingleton<KafkaConsumerFactory>();

        return services;
    }
}