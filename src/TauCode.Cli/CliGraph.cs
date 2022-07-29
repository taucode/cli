using TauCode.Data.Graphs;
using TauCode.Parsing;

namespace TauCode.Cli;

public class CliGraph : Graph
{
    public IParsingNode RootNode { get; internal set; } = null!;
}
