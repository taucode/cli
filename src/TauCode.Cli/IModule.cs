namespace TauCode.Cli;

public interface IModule : IDisposable
{
    string? Name { get; }

    IReadOnlyList<IExecutor> Executors { get; }

    bool Contains(IExecutor executor);

    void AddExecutor(IExecutor executor);

    IExecutor? GetExecutor(string? executorName);
}
