using TauCode.Cli.Tests.Common.Apps.Tau.LibDev.BumpVersion;
using TauCode.Cli.Tests.Common.Apps.Tau.LibDev.CheckDevRelease;

namespace TauCode.Cli.Tests.Common.Apps.Tau.LibDev;

public class LibDevModule : Module
{
    public LibDevModule()
        : base("libdev")
    {
        this.AddExecutors(
            new BumpVersionExecutor(),
            new CheckDevReleaseExecutor());
    }
}