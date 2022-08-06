using System.Text.RegularExpressions;
using TauCode.Cli.Tests.Common.Apps.Git.Tokens;
using TauCode.Parsing;

namespace TauCode.Cli.Tests.Common.Apps.Git.TokenProducers;

public class BranchNameProducer : ILexicalTokenProducer
{
    public ILexicalToken Produce(LexingContext context)
    {
        const string pattern = @"^[a-zA-Z\d]([a-zA-Z\d-]|/[a-zA-Z\d-])*";
        var start = context.Position;
        var input = context.Input[context.Position..];

        var match = Regex.Match(input.ToString(), pattern); // not much effective, but let it be
        if (match.Success)
        {
            var branchName = match.Groups[0].Value;

            var token = new BranchNameToken(start, branchName.Length, branchName);
            return token;
        }

        return null;
    }
}