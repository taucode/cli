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

    ExecutionContext BuildFromModule(
        IApp app,
        IModule module,
        ReadOnlyMemory<char> moduleInput,
        bool allowIncomplete);

    ExecutionContext BuildFromExecutor(
        IApp app,
        IModule module,
        IExecutor executor,
        ReadOnlyMemory<char> executorInput,
        bool allowIncomplete);

    ILogger? GetLogger();

    TextReader? GetInput();

    TextWriter? GetOutput();
}
