using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RecordNotFoundException : Exception
{
    public RecordNotFoundException(string message) : base(message)
    {
    }

    protected RecordNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}