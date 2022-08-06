using Serilog;

namespace TauCode.Cli;

public interface IAppHost
{
    IApp App { get; }

    ILogger? Logger { get; set; }

    TextReader? Input { get; set; }

    TextWriter? Output { get; set; }

    void Run(string[] args);

    Task RunAsync(string[] args, CancellationToken cancellationToken = default);
}