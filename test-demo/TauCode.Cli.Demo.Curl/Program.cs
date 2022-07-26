using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TauCode.Cli.Tests.Common.Apps.Curl;
using TauCode.Cli.Tests.Common.Apps.Git;

namespace TauCode.Cli.Demo.Curl;

public class Program
{
    public static void Main(
        string[] args) => CreateAppHost().Run(args);

    public static async Task Main(
        string[] args,
        double removeToMakeThisMethodAnEntryPoint) => await CreateAppHost().RunAsync(args);

    private static AppHost CreateAppHost()
    {
        var host = new AppHost(new CurlApp())
        {
            Input = Console.In,
            Output = Console.Out,
        };

        return host;
    }
}
