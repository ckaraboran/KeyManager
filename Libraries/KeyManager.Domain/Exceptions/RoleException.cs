using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class RoleException : Exception
{
    public RoleException(string message) : base(message)
    {
    }

    protected RoleException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}