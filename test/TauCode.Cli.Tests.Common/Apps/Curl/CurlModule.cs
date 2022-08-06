namespace TauCode.Cli.Tests.Common.Apps.Curl;

public class CurlModule : Module
{
    public CurlModule()
        : base(null)
    {
        this.AddExecutor(new CurlExecutor());
    }
}