namespace Producer.Model;

public class KafkaProducerResponse
{
    public string? Topic { get; set; }
    public int Partition { get; set; }
    public long Offset { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public IDictionary<string, string>? Headers { get; set; }
    public string? MessageJson { get; set; }
    public int MessageSize { get; set; }
}