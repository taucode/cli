using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeMetadata
{
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
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        protected override void ExecuteImplReal(
            Command command,
            IExecutionContext executionContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
