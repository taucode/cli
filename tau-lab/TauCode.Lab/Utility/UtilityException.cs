using System;
using System.Runtime.Serialization;

namespace TauCode.Lab.Utility
{
    [Serializable]
    public class UtilityException : Exception
    {
        public UtilityException()
        {
        }

        public UtilityException(string message)
            : base(message)
        {
        }

        public UtilityException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UtilityException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
