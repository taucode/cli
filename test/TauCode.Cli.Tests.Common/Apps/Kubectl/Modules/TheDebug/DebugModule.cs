namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.TheDebug;

public class DebugModule : KubectlModule
{
    public DebugModule()
        : base("debug")
    {
        this.AddExecutor(new DebugExecutor());
    }
}