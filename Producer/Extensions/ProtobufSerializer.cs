using Confluent.Kafka;
using Google.Protobuf;

namespace Producer.Extensions;

public class ProtobufSerializer<T> : ISerializer<T> where T : IMessage<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        if (data == null)
            return null;
        
        return data.ToByteArray();
    }
}