namespace TauCode.Cli.Executors;

public abstract class ExecutorBase : IExecutor
{
    protected ExecutorBase(string? name)
    {
        this.Name = name;
    }

    public string? Name { get; }

    public abstract void Execute(ExecutionContext executionContext);

    public abstract Task ExecuteAsync(ExecutionContext executionContext, CancellationToken cancellationToken = default);

    public virtual void Dispose()
    {
        // idle
    }
}