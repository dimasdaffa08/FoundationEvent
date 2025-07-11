namespace Producer.Configurations;

public class KafkaProducerProperties
{
    public string BootstrapServers { get; set; } = string.Empty;
    
    public string? ClientId { get; set; }
        
    public string Acks { get; set; } = "all";
        
    public int Retries { get; set; } = 3;
        
    public int BatchSize { get; set; } = 16384;
        
    public int LingerMs { get; set; } = 5;
        
    public string CompressionType { get; set; } = "none";
        
    public int RequestTimeoutMs { get; set; } = 30000;
        
    public int DeliveryTimeoutMs { get; set; } = 120000;
        
    public string SecurityProtocol { get; set; } = "plaintext";
        
    public string? SaslMechanism { get; set; }
        
    public string? SaslUsername { get; set; }
        
    public string? SaslPassword { get; set; }
        
    public string? SslCaLocation { get; set; }
        
    public Dictionary<string, string> AdditionalConfig { get; set; } = new();
        
    public bool EnableIdempotence { get; set; } = true;
        
    public int MaxInFlightRequestsPerConnection { get; set; } = 5;
}