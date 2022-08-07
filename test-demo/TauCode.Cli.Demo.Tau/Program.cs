using System;
using System.Threading.Tasks;
using TauCode.Cli.Tests.Common.Apps.Tau;

namespace TauCode.Cli.Demo.Tau;

public class Program
{
    public static void Main()
    {
        var args = CliHelper.GetCommandLineArguments();
        using var host = CreateAppHost();
        host.Run(args);
    }

    public static async Task MainCanBe()
    {
        var args = CliHelper.GetCommandLineArguments();
        await CreateAppHost().RunAsync(args);
    }

    private static AppHost CreateAppHost()
    {
        var host = new AppHost(new TauApp(true))
        {
            Input = Console.In,
            Output = Console.Out,
        };

        return host;
    }
}