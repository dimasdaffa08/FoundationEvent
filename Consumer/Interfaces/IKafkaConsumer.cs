namespace Consumer.Interfaces;

public interface IKafkaConsumer
{
    Task ListenAsync(string topic, CancellationToken cancellationToken = default);
}