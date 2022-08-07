using TauCode.Cli.Exceptions;

namespace TauCode.Cli.ReplCommandProcessors
{
    public class UseProcessor : ReplCommandProcessor
    {
        public UseProcessor(IReplHost host)
            : base(
                host,
                new[]
                {
                    "-u",
                    "--use",
                },
                "Use app / module / executor")
        {
        }

        public override void Process(ReplContext replContext)
        {
            var currentApp = this.Host.CurrentApp;
            var currentModule = this.Host.CurrentModule;
            var currentExecutor = this.Host.CurrentExecutor;

            try
            {
                if (replContext.RemainingInput.IsEmpty)
                {
                    this.Host.SetCurrentState(null, null, null);
                    return;
                }

                var context = this.Host.BuildFromSelf(replContext.RemainingInput, true);

                if (!context.Arguments.IsEmpty)
                {
                    throw new CliException("Executor arguments are not applicable here.");
                }

                this.Host.SetCurrentState(context.App, context.Module, context.Executor);
            }
            catch
            {
                this.Host.SetCurrentState(currentApp, currentModule, currentExecutor);
                replContext.Rewind();
                throw;
            }
        }
    }
}
