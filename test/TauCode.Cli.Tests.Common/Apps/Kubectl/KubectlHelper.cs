using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Kubectl;

public static class KubectlHelper
{
    public static readonly ILexer Lexer = new Lexer
    {
        Producers = new ILexicalTokenProducer[]
        {
            new WhiteSpaceProducer(),
        }
    };
}