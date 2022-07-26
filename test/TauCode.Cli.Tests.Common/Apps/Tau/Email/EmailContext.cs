using Serilog;
using System.IO;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email
{
    public class EmailContext : ExecutionContextBase
    {
        public EmailContext(
            ILogger logger,
            TextReader input,
            TextWriter output)
            : base(
                logger,
                input,
                output)
        {
        }

        protected override void DisposeImpl()
        {
            // todo
        }
    }
}
