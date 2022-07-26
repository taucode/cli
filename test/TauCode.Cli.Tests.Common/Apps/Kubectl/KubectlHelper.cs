using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Kubectl
{
    public static class KubectlHelper
    {
        public static readonly ILexer KubectlLexer = new Lexer
        {
            Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(),
            }
        };

        private static readonly CliGraphScriptReader ScriptReader =
            new CliGraphScriptReader(new CliVertexFactory(new CliTokenTypeResolver()));


        public static IGraph BuildParsingGraph(string resourceName)
        {
            var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
            var graph = ScriptReader.BuildGraph(script);

            return graph;
        }
    }
}
