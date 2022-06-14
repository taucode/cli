namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Create
{
    public class CreateModule : KubectlModule
    {
        public CreateModule()
            : base("create")
        {
            this.AddExecutor(new CreateExecutor());
        }
    }
}
