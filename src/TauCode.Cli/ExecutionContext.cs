using Serilog;

namespace TauCode.Cli;

public class ExecutionContext
{
    public ExecutionContext(
        IApp? app,
        IModule? module,
        IExecutor? executor,
        string? executorName,
        ReadOnlyMemory<char>? rawArguments,
        string[]? arguments,
        ILogger? logger,
        TextReader? input,
        TextWriter? output)
    {
        this.App = app;
        this.Module = module;
        this.Executor = executor;
        this.ExecutorName = executorName;

        var onlyOne = rawArguments == null ^ arguments == null;
        if (!onlyOne)
        {
            throw new ArgumentException(
                $"Either '{nameof(rawArguments)}' or '{nameof(arguments)}' must be non-null.",
                $"{nameof(rawArguments)}/{nameof(arguments)}");
        }

        this.RawArguments = rawArguments;
        this.Arguments = arguments;

        this.Logger = logger;
        this.Input = input;
        this.Output = output;
    }

    public IApp? App { get; }
    public IModule? Module { get; }
    public IExecutor? Executor { get; }
    public string? ExecutorName { get; }

    public ReadOnlyMemory<char>? RawArguments { get; }
    public string[]? Arguments { get; }

    public ILogger? Logger { get; set; }
    public TextReader? Input { get; set; }
    public TextWriter? Output { get; set; }
}
