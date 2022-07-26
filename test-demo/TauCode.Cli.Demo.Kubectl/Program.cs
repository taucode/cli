using System;
using System.Threading.Tasks;
using TauCode.Cli.Tests.Common.Apps.Kubectl;

namespace TauCode.Cli.Demo.Kubectl;

public class Program
{
    public static void Main(
        string[] args,
        double removeToMakeThisMethodAnEntryPoint) => CreateAppHost().Run(args);

    public static async Task Main(
        string[] args) => await CreateAppHost().RunAsync(args);

    private static AppHost CreateAppHost()
    {
        var host = new AppHost(new KubectlApp())
        {
            Input = Console.In,
            Output = Console.Out,
        };

        return host;
    }
}