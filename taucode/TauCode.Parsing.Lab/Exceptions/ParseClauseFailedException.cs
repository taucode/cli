﻿using System;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lab.Exceptions
{
    public class ParseClauseFailedException : ParsingException
    {
        protected ParseClauseFailedException(string message, object[] partialParsingResults)
            : base(message)
        {
            this.PartialParsingResults = partialParsingResults;
        }

        protected ParseClauseFailedException(string message, Exception innerException, object[] partialParsingResults)
            : base(message, innerException)
        {
            this.PartialParsingResults = partialParsingResults;
        }

        public object[] PartialParsingResults { get; }
    }
}