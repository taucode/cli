using System.Linq;
using System.Text.RegularExpressions;
using TauCode.Cli.Tests.Common.Apps.Git.TokenProducers;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Git
{
    // todo clean
    public static class GitHelper
    {
        private static readonly CliGraphScriptReader ScriptReader =
            new CliGraphScriptReader(new CliVertexFactory(new GitTokenTypeResolver()));

        private static readonly GitTokenConverter TokenConverter = new GitTokenConverter();

        public static IGraph BuildParsingGraph(string resourceName)
        {
            var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
            var graph = ScriptReader.BuildGraph(script);

            var nodesWhichCanConvertTokens = graph
                .Where(x => !x.GetType().IsIn(typeof(IdleNode), typeof(EndNode)))
                .Cast<IParsingNode>()
                .ToList();

            nodesWhichCanConvertTokens.ForEach(x => x.TokenConverter = TokenConverter);

            return graph;
        }

        internal static IParsingNode BuildParsingNode(string resourceName)
        {
            var graph = BuildParsingGraph(resourceName);
            var node = ScriptReader.ResolveParsingNode(graph);

            return node;
        }

        public static bool IsValidBranchName(string text)
        {
            const string pattern = @"^[a-zA-Z\d]([a-zA-Z\d-]|/[a-zA-Z\d-])*$";
            return Regex.IsMatch(text, pattern);
        }

        public static bool IsValidRefName(string text)
        {
            const string pattern = @"^([0-9a-f]{7}|HEAD(~\d+)?)$";
            return Regex.IsMatch(text, pattern);
        }

        public static ILexer CreateGitLexer()
        {
            var lexer = new Lexer
            {
                Producers = new ILexicalTokenProducer[]
                {
                    new WhiteSpaceProducer(),
                    // todo place FilePathProducer here to test token conversion.

                    new KeyProducer(CliHelper.IsCliWhiteSpace),
                    new UriProducer(CliHelper.IsCliWhiteSpace),
                    new BranchNameProducer(),
                    new RefNameProducer(),
                    new FilePathProducer(CliHelper.IsCliWhiteSpace),
                },
            };

            return lexer;
        }
    }
}
