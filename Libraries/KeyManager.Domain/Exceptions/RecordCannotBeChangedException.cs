using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RecordCannotBeChangedException : Exception
{
    public RecordCannotBeChangedException(string message) : base(message)
    {
    }

    protected RecordCannotBeChangedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}