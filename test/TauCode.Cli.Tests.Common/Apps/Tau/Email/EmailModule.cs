using TauCode.Cli.Tests.Common.Apps.Tau.Email.Send;
using TauCode.Cli.Tests.Common.Apps.Tau.Email.Settings;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email;

public class EmailModule : Module
{
    public EmailModule()
        : base("email")
    {
        this.AddExecutors(
            new SendExecutor(),
            new SettingsExecutor());
    }
}