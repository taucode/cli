using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.PurgeData
{
    public class PurgeDataExecutor : DbExecutor
    {
        public PurgeDataExecutor(DbModule dbModule)
            : base(
                "purge-data",
                $".{nameof(PurgeDataExecutor)}.lisp",
                dbModule)
        {
        }

        protected override Task ExecuteImplRealAsync(Command command, IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        protected override void ExecuteImplReal(Command command, IExecutionContext executionContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
