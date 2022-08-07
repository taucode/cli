using System;
using System.Threading.Tasks;
using TauCode.Cli.Tests.Common.Apps.Git;

namespace TauCode.Cli.Demo.Git;

public class Program
{
    public static void Main()
    {
        var args = CliHelper.GetCommandLineArguments();
        CreateAppHost().Run(args);
    }

    public static async Task MainCanBe()
    {
        var args = CliHelper.GetCommandLineArguments();
        await CreateAppHost().RunAsync(args);
    }

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
