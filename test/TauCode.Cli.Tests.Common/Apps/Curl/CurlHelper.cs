using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Curl;

public static class CurlHelper
{
    public static readonly ILexer Lexer = new Lexer
    {
        Producers = new ILexicalTokenProducer[]
        {
            new WhiteSpaceProducer(),
            new JsonStringProducer(CliHelper.IsCliWhiteSpace),
            new KeyProducer(CliHelper.IsCliWhiteSpace),
            new UriProducer(CliHelper.IsCliWhiteSpace),
        }
    };
}