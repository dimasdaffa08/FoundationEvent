using Google.Protobuf;
using Producer.Model;

namespace Producer.Interfaces;

public interface IKafkaProducer<TKey, TValue> : IDisposable
    where TValue : IMessage<TValue>
{
    Task<KafkaProducerResponse> SendAsync(string topic, TKey key, TValue value, CancellationToken cancellationToken = default);
    
    Task<KafkaProducerResponse> SendAsync(string topic, TKey key, TValue value, IDictionary<string, string> headers, CancellationToken cancellationToken = default);
    Task<KafkaProducerResponse> SendAsync(string topic, TValue value, IDictionary<string, string> headers, CancellationToken cancellationToken = default);
}