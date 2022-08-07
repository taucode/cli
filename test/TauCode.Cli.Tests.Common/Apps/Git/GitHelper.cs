using System.Text.RegularExpressions;
using TauCode.Cli.Tests.Common.Apps.Git.TokenProducers;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Git;

public static class GitHelper
{
    private static readonly CliGraphParser GraphParser = new(
        new CliGraphScriptReader(),
        new CliGraphBuilder(
            new CliVertexFactory(new GitTokenTypeResolver())));

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

    public static ILexer Lexer = new Lexer
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

    public static CliGraph BuildCliGraph(string graphResourceName)
    {
        var script = typeof(GitHelper).Assembly.GetResourceText(graphResourceName, true);
        var graph = GraphParser.ParseScript(script);
        return graph;
    }
}
