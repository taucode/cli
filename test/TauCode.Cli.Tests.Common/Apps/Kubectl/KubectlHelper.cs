using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Kubectl;

// todo clean
public static class KubectlHelper
{
    public static readonly ILexer Lexer = new Lexer
    {
        // todo 
        Producers = new ILexicalTokenProducer[]
        {
            new WhiteSpaceProducer(),
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