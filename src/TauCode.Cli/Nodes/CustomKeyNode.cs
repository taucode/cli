using System.Text;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Nodes;

public class CustomKeyNode : ActionNode
{
    public CustomKeyNode(
        IEnumerable<string> keys)
    {
        // todo checks
        this.Keys = new HashSet<string>(keys);
    }

    public HashSet<string> Keys { get; }

    protected override bool AcceptsImpl(ParsingContext parsingContext)
    {
        var token = parsingContext.GetCurrentToken();

        KeyToken keyToken;
        if (this.TokenConverter == null)
        {
            keyToken = token as KeyToken;
        }
        else
        {
            keyToken = this.TokenConverter.Convert<KeyToken>(token, parsingContext.ParsingResult);
        }

        return keyToken != null && this.Keys.Contains(keyToken.Text);
    }

    protected override string GetDataTag()
    {
        var sb = new StringBuilder();
        sb.Append($"Keys: ");
        sb.Append(string.Join(", ", this.Keys.Select(x => $"'{x}'")));

        return sb.ToString();
    }
}