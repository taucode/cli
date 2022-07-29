using TauCode.Parsing;

namespace TauCode.Cli;

public class CliGraphParser
{
    private readonly CliGraphScriptReader _scriptReader;
    private readonly CliGraphBuilder _graphBuilder;

    public CliGraphParser(CliGraphScriptReader scriptReader, CliGraphBuilder graphBuilder)
    {
        _scriptReader = scriptReader;
        _graphBuilder = graphBuilder;
    }

    public CliGraph ParseScript(string script)
    {
        if (script == null)
        {
            throw new ArgumentNullException(nameof(script));
        }

        var mold = _scriptReader.ReadScript(script.AsMemory());
        var graph = (CliGraph)_graphBuilder.Build(mold);

        graph.RootNode = (IParsingNode)graph.Single(x => x.Name == "parsing-root");
        return graph;
    }
}