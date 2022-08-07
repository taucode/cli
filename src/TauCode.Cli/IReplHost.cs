using Serilog;

namespace TauCode.Cli;

public interface IReplHost : IExecutionContextBuilder, IDisposable
{
    IReadOnlyList<IApp> Apps { get; }

    void AddApp(IApp app);

    IApp GetApp(string appName);

    IApp? CurrentApp { get; }

    IModule? CurrentModule { get; }

    IExecutor? CurrentExecutor { get; }

    IReadOnlyList<ReplCommandProcessor> CommandProcessors { get; }

    ILogger? Logger { get; set; }

    TextReader? Input { get; set; }

    TextWriter? Output { get; set; }

    void Run(string[]? script = null);

    Task RunAsync(
        string[]? script = null,
        CancellationToken cancellationToken = default);

    void SetCurrentState(IApp? currentApp, IModule? currentModule, IExecutor? currentExecutor);
}
