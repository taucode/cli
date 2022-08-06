using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.PurgeData;

public class PurgeDataExecutor : DbExecutor
{
    public PurgeDataExecutor(DbModule dbModule)
        : base(
            "purge-data",
            $".{nameof(PurgeDataExecutor)}.lisp",
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