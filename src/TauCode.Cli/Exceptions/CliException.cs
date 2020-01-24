﻿using System;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class CliException : Exception
    {
        public CliException(string message)
            : base(message)
        {
        }

        public CliException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
