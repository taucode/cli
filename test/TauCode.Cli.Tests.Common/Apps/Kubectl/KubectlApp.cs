using TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Create;
using TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Run;
using TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.TheDebug;

namespace TauCode.Cli.Tests.Common.Apps.Kubectl
{
    public class KubectlApp : App
    {
        public KubectlApp()
            : base("kubectl")
        {
            this.AddModule(new CreateModule());
            this.AddModule(new RunModule());
            this.AddModule(new DebugModule());
        }
    }
}
