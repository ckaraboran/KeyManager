using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace KeyManager.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class UserException : Exception
{
    public UserException(string message) : base(message)
    {
    }

    protected UserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}