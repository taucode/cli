using Serilog;
using System.IO;
using TauCode.Cli.Tests.Common.Apps.Git.Executors.Branch;
using TauCode.Cli.Tests.Common.Apps.Git.Executors.Checkout;
using TauCode.Cli.Tests.Common.Apps.Git.Executors.Clone;

namespace TauCode.Cli.Tests.Common.Apps.Git
{
    public class GitModule : Module
    {
        public GitModule()
            : base(null, GitHelper.CreateGitLexer())
        {
            this.AddExecutor(new CheckoutExecutor());
            this.AddExecutor(new BranchExecutor());
            this.AddExecutor(new CloneExecutor());
        }

        public override IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
