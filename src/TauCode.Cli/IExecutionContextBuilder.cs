using Serilog;

namespace TauCode.Cli;

public interface IExecutionContextBuilder
{
    ExecutionContext BuildFromSelf(
        ReadOnlyMemory<char> input,
        bool allowIncomplete);

    ExecutionContext BuildFromApp(
        IApp app,
        ReadOnlyMemory<char> appInput,
        bool allowIncomplete);

    ExecutionContext BuildFromApp(
        IApp app,
        string[] appArgs,
        bool allowIncomplete);

    ExecutionContext BuildFromModule(
        IApp app,
        IModule module,
        ReadOnlyMemory<char> moduleInput,
        bool allowIncomplete);

    ExecutionContext BuildFromModule(
        IApp app,
        IModule module,
        string[] moduleArgs,
        bool allowIncomplete);

    ExecutionContext BuildFromExecutor(
        IApp app,
        IModule module,
        IExecutor executor,
        ReadOnlyMemory<char> executorInput,
        bool allowIncomplete);

    ExecutionContext BuildFromExecutor(
        IApp app,
        IModule module,
        IExecutor executor,
        string[] executorArgs,
        bool allowIncomplete);

    ILogger? Logger { get; }
    TextReader? Input { get; }
    TextWriter? Output { get; }
}
