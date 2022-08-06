using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Executors;
using TauCode.Parsing;

namespace TauCode.Cli.Tests.Common;

public class TestExecutorBase : ParsingExecutorBase
{
    public TestExecutorBase(
        string? name,
        ILexer lexer,
        string graphResourceName)
        : base(
            name,
            lexer,
            Helper.BuildCliGraph(graphResourceName))
    {
    }

    public TestExecutorBase(
        string? name,
        ILexer lexer,
        CliGraph graph)
        : base(
            name,
            lexer,
            graph)
    {
    }

    protected override Task ExecuteImplAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken = default)
    {
        Helper.Dump(command, executionContext.Output!);
        return Task.CompletedTask;
    }
}