using System;
using System.Runtime.Serialization;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class CliException : Exception
    {
        public CliException()
        {
        }

        public CliException(string message)
            : base(message)
        {
        }

        public CliException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CliException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
