using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.TableInfo;

public class TableInfoExecutor : DbExecutor
{
    public TableInfoExecutor(DbModule dbModule)
        : base(
            "table-info",
            $".{nameof(TableInfoExecutor)}.lisp",
            dbModule)
    {
    }

    protected override Task ExecuteImplRealAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
