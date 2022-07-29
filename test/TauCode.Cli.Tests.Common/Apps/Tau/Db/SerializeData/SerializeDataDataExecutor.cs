using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeData;

public class SerializeDataDataExecutor : DbExecutor
{
    public SerializeDataDataExecutor(DbModule dbModule)
        : base(
            "serialize-data",
            $".{nameof(SerializeDataDataExecutor)}.lisp",
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