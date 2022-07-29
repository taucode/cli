namespace TauCode.Cli;

public interface IReplHost : IExecutionContextBuilder
{
    IReadOnlyList<IApp> Apps { get; }

    void AddApp(IApp app);

    IApp GetApp(string appName);

    IApp? CurrentApp { get; }

    IModule? CurrentModule { get; }

    IExecutor? CurrentExecutor { get; }
    //string? CurrentExecutorName { get; }

    IReadOnlyList<ReplCommandProcessor> CommandProcessors { get; }

    void Run(string[]? script = null);

    Task RunAsync(
        string[]? script = null,
        CancellationToken cancellationToken = default);

    void SetCurrentState(IApp? currentApp, IModule? currentModule, IExecutor? currentExecutor);
}
