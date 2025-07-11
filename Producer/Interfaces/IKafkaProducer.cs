using Producer.Model;

namespace Producer.Interfaces;

public interface IKafkaProducer<TKey, TValue> : IDisposable
{
    Task<KafkaProducerResponse> SendAsync(string topic, TKey key, TValue value, CancellationToken cancellationToken = default);
    
    Task<KafkaProducerResponse> SendAsync(string topic, TKey key, TValue value, IDictionary<string, string> headers, CancellationToken cancellationToken = default);
}