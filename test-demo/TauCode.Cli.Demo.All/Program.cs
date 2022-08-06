using System;
using System.Threading.Tasks;
using TauCode.Cli.Tests.Common;

namespace TauCode.Cli.Demo.All;

public class Program
{
    public static async Task Main(string[] args)
    {
        var replHost = new TauReplHost(Console.In, Console.Out, true);

        await replHost.RunAsync();
    }
}
