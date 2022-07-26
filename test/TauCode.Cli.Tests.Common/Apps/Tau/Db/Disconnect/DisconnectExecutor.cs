using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.Disconnect
{
    public class DisconnectExecutor : DbExecutor
    {
        public DisconnectExecutor(DbModule dbModule)
            : base(
                "disconnect",
                $".{nameof(DisconnectExecutor)}.lisp",
                dbModule)
        {
        }

        protected override Task ExecuteImplRealAsync(
            Command command,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            this.ExecuteImplReal(command, executionContext);

            return Task.CompletedTask;
        }

        protected override void ExecuteImplReal(Command command, IExecutionContext executionContext)
        {
            this.DbModule.CurrentExecutionContext?.Dispose();
            this.DbModule.CurrentExecutionContext = null;
        }
    }
}
