using System;
using System.Collections.Generic;
using TauCode.Parsing;

namespace TauCode.Cli.Nodes
{
    public class KeyValueNode : ParsingNodeBase
    {
        public KeyValueNode(
            string alias,
            IEnumerable<Type> tokenTypes)
        {
            // todo checks

            this.Alias = alias;
            this.TokenTypes = new HashSet<Type>(tokenTypes);
        }

        public string Alias { get; }

        public HashSet<Type> TokenTypes { get; }

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            var parsingResult = parsingContext.ParsingResult;

            if (this.TokenTypes.Contains(token.GetType()))
            {
                return true;
            }

            if (this.TokenConverter != null)
            {
                foreach (var tokenType in this.TokenTypes)
                {
                    var convertedToken = this.TokenConverter.Convert(token, tokenType, parsingResult);
                    if (convertedToken != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected override void ActImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            var parsingResult = parsingContext.ParsingResult;

            var command = (Command)parsingResult;

            command.KeyValues[this.Alias].Add(token);
            command.IncreaseVersion();
        }

        protected override string GetDataTag()
        {
            return this.Alias;
        }
    }
}
