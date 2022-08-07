using System.Runtime.Serialization;

namespace TauCode.Cli.Exceptions;

[Serializable]
public class ExitReplException : CliException
{
    public ExitReplException()
    {
    }

    public ExitReplException(string message)
        : base(message)
    {
    }

    public ExitReplException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected ExitReplException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}