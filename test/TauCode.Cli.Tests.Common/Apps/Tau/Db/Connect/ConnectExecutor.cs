using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.Connect;

public class ConnectExecutor : DbExecutor
{
    public ConnectExecutor(DbModule dbModule)
        : base(
            "connect",
            $".{nameof(ConnectExecutor)}.lisp",
            dbModule)
    {
    }

    protected override Task ExecuteImplRealAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        var provider = command.KeyValues["provider"].Single()!.ToString()!.ToEnum<DbProvider>();
        var connectionString = command.KeyValues["connection"].Single().ToString();

        var connection = DbHelper.CreateConnection(connectionString!, provider);

        this.DbModule.Connection?.Dispose();
        this.DbModule.Connection = connection;

        return Task.CompletedTask;
    }
}
