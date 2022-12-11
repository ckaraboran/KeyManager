using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class UserRoleException : Exception
{
    public UserRoleException(string message) : base(message)
    {
    }

    protected UserRoleException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}