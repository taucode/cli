namespace TauCode.Cli.Tests.Common.Apps.Tau
{
    public class TauAppHost : AppHost
    {
        public TauAppHost(bool isDbReal)
            : base(new TauApp(isDbReal))
        {
        }
    }
}
