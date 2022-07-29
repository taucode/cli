using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.Disconnect;

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
        ExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        this.DbModule.Connection?.Dispose();
        this.DbModule.Connection = null;

        return Task.CompletedTask;
    }
}
