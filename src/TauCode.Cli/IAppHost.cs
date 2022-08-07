using Serilog;

namespace TauCode.Cli;

public interface IAppHost : IDisposable
{
    IApp App { get; }

    ILogger? Logger { get; set; }

    TextReader? Input { get; set; }

    TextWriter? Output { get; set; }

    void Run(ReadOnlyMemory<char> input);

    Task RunAsync(ReadOnlyMemory<char> input, CancellationToken cancellationToken = default);
}