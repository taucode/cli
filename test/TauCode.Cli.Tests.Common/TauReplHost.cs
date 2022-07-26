using TauCode.Cli.Tests.Common.Apps;
using TauCode.Cli.Tests.Common.Apps.Curl;
using TauCode.Cli.Tests.Common.Apps.Git;
using TauCode.Cli.Tests.Common.Apps.Kubectl;
using TauCode.Cli.Tests.Common.Apps.Tau;

namespace TauCode.Cli.Tests.Common
{
    public class TauReplHost : ReplHost
    {
        public TauReplHost(bool isDbReal)
        {
            this.AddApp(new TauApp(isDbReal));
            this.AddApp(new KubectlApp());
            this.AddApp(new GitApp());
            this.AddApp(new CurlApp());
        }

        protected override string CetContextDescription(IExecutionContext executionContext)
        {
            var ctx = (TestExecutionContextBase)executionContext;
            return ctx.GetDescription();
        }
    }
}
