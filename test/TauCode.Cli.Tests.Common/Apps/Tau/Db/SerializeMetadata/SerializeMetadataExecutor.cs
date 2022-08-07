using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeMetadata;

public class SerializeMetadataExecutor : DbExecutor
{
    public SerializeMetadataExecutor(DbModule dbModule)
        : base(
            "serialize-metadata",
            $".{nameof(SerializeMetadataExecutor)}.lisp",
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
