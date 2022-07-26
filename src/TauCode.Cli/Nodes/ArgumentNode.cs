using System;
using System.Collections.Generic;
using TauCode.Parsing;

namespace TauCode.Cli.Nodes
{
    public class ArgumentNode : ParsingNodeBase
    {
        public ArgumentNode(
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

            if (this.TokenTypes.Contains(token.GetType()))
            {
                return true;
            }

            if (this.TokenConverter != null)
            {
                foreach (var tokenType in this.TokenTypes)
                {
                    var convertedToken = this.TokenConverter.Convert(token, tokenType, parsingContext.ParsingResult);
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

            var containsAlias = command.Arguments.TryGetValue(this.Alias, out var list);
            if (containsAlias)
            {
                list.Add(token);
            }
            else
            {
                list = new List<ILexicalToken>
                {
                    token,
                };

                command.Arguments.Add(this.Alias, list);
            }

            command.IncreaseVersion();
        }

        protected override string GetDataTag()
        {
            return this.Alias;
        }
    }
}
