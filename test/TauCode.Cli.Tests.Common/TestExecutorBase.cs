using System;
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

    protected override Command CreateCommand(string? executorName, ReadOnlyMemory<char> input)
    {
        return new TestCommand(executorName, input);
    }

    protected override void ExecuteImpl(Command command, ExecutionContext executionContext)
    {
        Helper.Dump((TestCommand)command, executionContext.Output!);
    }

    protected override Task ExecuteImplAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken = default)
    {
        Helper.Dump((TestCommand)command, executionContext.Output!);
        return Task.CompletedTask;
    }
}