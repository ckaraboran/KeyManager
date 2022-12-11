using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class PermissionException : Exception
{
    public PermissionException(string message) : base(message)
    {
    }

    protected PermissionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}