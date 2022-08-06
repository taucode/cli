namespace TauCode.Cli;

public interface IModule
{
    string? Name { get; }

    IReadOnlyList<IExecutor> Executors { get; }

    bool Contains(IExecutor executor);

    void AddExecutor(IExecutor executor);

    IExecutor? GetExecutor(string? executorName);
}
