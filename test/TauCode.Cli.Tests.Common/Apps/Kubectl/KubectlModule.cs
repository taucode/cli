using Serilog;
using System.IO;

namespace TauCode.Cli.Tests.Common.Apps.Kubectl
{
    public class KubectlModule : Module
    {
        public KubectlModule(string name)
            : base(name, KubectlHelper.KubectlLexer)
        {
        }

        public override IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
