namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Settings
{
    public class SettingsExecutor : TestExecutorBase
    {
        public SettingsExecutor()
            : base(
                "settings",
                TauHelper.BuildParsingGraph($".{nameof(SettingsExecutor)}.lisp"))
        {
        }
    }
}
