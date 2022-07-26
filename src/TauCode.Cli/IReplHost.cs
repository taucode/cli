using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli
{
    public interface IReplHost
    {
        IReadOnlyList<IApp> Apps { get; }

        void AddApp(IApp app);

        IApp GetApp(string appName);

        IApp CurrentApp { get; }

        IModule CurrentModule { get; }

        IExecutor CurrentExecutor { get; }

        ILogger Logger { get; set; }

        TextReader Input { get; set; }

        TextWriter Output { get; set; }

        void Run();

        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
