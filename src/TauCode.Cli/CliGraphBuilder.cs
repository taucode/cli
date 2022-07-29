using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Building.Impl;

namespace TauCode.Cli;

public class CliGraphBuilder : GraphBuilder
{
    public CliGraphBuilder(CliVertexFactory cliVertexFactory)
        : base(cliVertexFactory)
    {
    }

    protected override IGraph CreateGraph()
    {
        return new CliGraph();
    }
}