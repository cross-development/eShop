using System.Runtime.Serialization;

namespace Infrastructure.Exceptions;

public sealed class BusinessException : Exception
{
    public BusinessException()
    {
    }

    public BusinessException(string message)
        : base(message)
    {
    }

    public BusinessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public BusinessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}