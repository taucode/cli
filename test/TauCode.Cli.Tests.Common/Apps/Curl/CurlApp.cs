namespace TauCode.Cli.Tests.Common.Apps.Curl;

public class CurlApp : App
{
    public CurlApp()
        : base("curl")
    {
        this.AddModule(new CurlModule());
    }
}