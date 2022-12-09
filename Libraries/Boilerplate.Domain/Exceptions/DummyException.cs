using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Boilerplate.Domain.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class DummyException : Exception
{
    public DummyException(string message) : base(message)
    {
    }

    protected DummyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}