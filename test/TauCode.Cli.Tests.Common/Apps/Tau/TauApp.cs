using TauCode.Cli.Tests.Common.Apps.Tau.Db;
using TauCode.Cli.Tests.Common.Apps.Tau.Email;
using TauCode.Cli.Tests.Common.Apps.Tau.LibDev;

namespace TauCode.Cli.Tests.Common.Apps.Tau
{
    public class TauApp : App
    {
        public TauApp(bool isDbReal)
            : base("tau")
        {
            this.AddModules(
                new DbModule(isDbReal),
                new EmailModule(),
                new LibDevModule());
        }
    }
}
