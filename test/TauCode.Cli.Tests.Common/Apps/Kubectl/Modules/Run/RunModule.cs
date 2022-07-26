namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Run
{
    public class RunModule : KubectlModule
    {
        public RunModule()
            : base("run")
        {
            this.AddExecutor(new RunExecutor());
        }
    }
}
