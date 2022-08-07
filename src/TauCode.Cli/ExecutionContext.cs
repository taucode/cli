using Serilog;

namespace TauCode.Cli;

public class ExecutionContext
{
    public ExecutionContext(
        IApp? app,
        IModule? module,
        IExecutor? executor,
        string? executorName,
        ReadOnlyMemory<char> arguments,
        ILogger? logger,
        TextReader? input,
        TextWriter? output)
    {
        this.App = app;
        this.Module = module;
        this.Executor = executor;
        this.ExecutorName = executorName;

        this.Arguments = arguments;

        this.Logger = logger;
        this.Input = input;
        this.Output = output;
    }

    public IApp? App { get; }
    public IModule? Module { get; }
    public IExecutor? Executor { get; }
    public string? ExecutorName { get; }

    public ReadOnlyMemory<char> Arguments { get; }

    public ILogger? Logger { get; }
    public TextReader? Input { get; }
    public TextWriter? Output { get; }
}
