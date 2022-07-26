using Serilog;
using System.IO;
using TauCode.Cli.Tests.Common.Apps.Tau.Email.Send;
using TauCode.Cli.Tests.Common.Apps.Tau.Email.Settings;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email
{
    public class EmailModule : Module
    {
        public EmailModule()
            : base("email", EmailHelper.Lexer)
        {
            this.AddExecutors(
                new SendExecutor(),
                new SettingsExecutor());
        }

        public override IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output)
        {
            return new EmailContext(logger, input, output);
        }
    }
}
