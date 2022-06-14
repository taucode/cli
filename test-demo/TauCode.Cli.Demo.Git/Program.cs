using System;
using System.Threading.Tasks;
using TauCode.Cli.Tests.Common.Apps.Git;

namespace TauCode.Cli.Demo.Git;

public class Program
{
    public static void Main(
        string[] args,
        double removeToMakeThisMethodAnEntryPoint) => CreateAppHost().Run(args);

    public static async Task Main(
        string[] args) => await CreateAppHost().RunAsync(args);

    private static AppHost CreateAppHost()
    {
        var host = new AppHost(new GitApp())
        {
            Input = Console.In,
            Output = Console.Out,
        };

        return host;
    }
}
