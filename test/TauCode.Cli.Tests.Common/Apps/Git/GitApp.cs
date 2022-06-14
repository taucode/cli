namespace TauCode.Cli.Tests.Common.Apps.Git
{
    public class GitApp : App
    {
        public GitApp()
            : base("git")
        {
            this.AddModule(new GitModule());
        }
    }
}
