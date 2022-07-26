using Serilog;
using System.IO;

namespace TauCode.Cli.Tests.Common.Apps.Curl
{
    public class CurlModule : Module
    {
        public CurlModule()
            : base(null, CurlHelper.Lexer)
        {
            this.AddExecutor(new CurlExecutor());
        }

        public override IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output)
        {
            return new CurlContext(
                logger,
                input,
                output);
        }
    }
}
