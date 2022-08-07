namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Settings;

public class SettingsExecutor : TestExecutorBase
{
    public SettingsExecutor()
        : base(
            "settings",
            EmailHelper.Lexer,
            $".{nameof(SettingsExecutor)}.lisp")
    {
    }
}
