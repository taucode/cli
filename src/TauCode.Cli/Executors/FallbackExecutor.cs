namespace TauCode.Cli.Executors;

public abstract class FallbackExecutor : IExecutor
{
    public string Name { get; set; } = null!;

    public abstract void Execute(ExecutionContext executionContext);

    public abstract Task ExecuteAsync(ExecutionContext executionContext, CancellationToken cancellationToken = default);

    public abstract bool AcceptsName(string executorName);

    public override string? ToString()
    {
        return this.Name;
    }
}