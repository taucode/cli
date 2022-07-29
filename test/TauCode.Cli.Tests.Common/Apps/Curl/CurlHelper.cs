using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Curl;

// todo clean
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

    //private static readonly CliGraphScriptReader ScriptReader =
    //    new CliGraphScriptReader(new CliVertexFactory(new CliTokenTypeResolver()));


    //public static IGraph BuildParsingGraph(string resourceName)
    //{
    //    var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
    //    var graph = ScriptReader.BuildGraph(script);

    //    return graph;
    //}
}