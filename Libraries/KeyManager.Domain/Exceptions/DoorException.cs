using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class DoorException : Exception
{
    public DoorException(string message) : base(message)
    {
    }

    protected DoorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}