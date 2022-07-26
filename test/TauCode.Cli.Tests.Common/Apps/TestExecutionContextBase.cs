using Serilog;
using System.IO;

namespace TauCode.Cli.Tests.Common.Apps
{
    public abstract class TestExecutionContextBase : ExecutionContextBase
    {
        protected TestExecutionContextBase(
            ILogger logger,
            TextReader input,
            TextWriter output)
            : base(
                logger,
                input,
                output)
        {
        }

        public abstract string GetDescription();
    }
}
