using Serilog;
using System.IO;

namespace TauCode.Cli.Tests.Common.Apps.Curl
{
    public class CurlContext : ExecutionContextBase
    {
        public CurlContext(
            ILogger logger,
            TextReader input,
            TextWriter output)
            : base(logger, input, output)
        {
        }

        protected override void DisposeImpl()
        {
            // idle
        }
    }
}
