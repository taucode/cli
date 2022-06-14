using Serilog;
using System.IO;
using TauCode.Cli.Tests.Common.Apps.Tau.LibDev.BumpVersion;
using TauCode.Cli.Tests.Common.Apps.Tau.LibDev.CheckDevRelease;

namespace TauCode.Cli.Tests.Common.Apps.Tau.LibDev
{
    public class LibDevModule : Module
    {
        public LibDevModule()
            : base("libdev", LibDevHelper.Lexer)
        {
            this.AddExecutors(
                new BumpVersionExecutor(),
                new CheckDevReleaseExecutor());
        }

        public override IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output)
        {
            return new LibDevExecutionContext(logger, input, output);
        }
    }
}
