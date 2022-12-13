using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RecordCannotBeDeletedException : Exception
{
    public RecordCannotBeDeletedException(string message) : base(message)
    {
    }

    protected RecordCannotBeDeletedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}