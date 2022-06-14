using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Nodes
{
    public class KeyNode : ParsingNodeBase
    {
        public KeyNode(
            string alias,
            IEnumerable<string> keys)
        {
            // todo checks
            this.Alias = alias;
            this.Keys = new HashSet<string>(keys);
        }

        public string Alias { get; }

        public HashSet<string> Keys { get; }

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            var parsingResult = parsingContext.ParsingResult;

            KeyToken keyToken;
            if (this.TokenConverter == null)
            {
                keyToken = token as KeyToken;
            }
            else
            {
                keyToken = this.TokenConverter.Convert<KeyToken>(token, parsingResult);
            }

            return keyToken != null && this.Keys.Contains(keyToken.Text);
        }

        protected override void ActImpl(ParsingContext parsingContext)
        {
            var parsingResult = parsingContext.ParsingResult;
            var command = (Command)parsingResult;

            if (command.KeyValues.ContainsKey(this.Alias))
            {
                command.IncreaseVersion();
                return; // todo: check uniqueness
            }

            command.KeyValues.Add(this.Alias, new List<ILexicalToken>());
            command.IncreaseVersion();
        }

        protected override string GetDataTag()
        {
            var sb = new StringBuilder();
            sb.Append($"Alias: '{this.Alias}' Keys: ");
            sb.Append(string.Join(", ", this.Keys.Select(x => $"'{x}'")));

            return sb.ToString();
        }
    }
}
