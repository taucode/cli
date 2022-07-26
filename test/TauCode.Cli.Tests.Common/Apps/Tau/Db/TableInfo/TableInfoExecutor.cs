using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.TableInfo
{
    public class TableInfoExecutor : DbExecutor
    {
        public TableInfoExecutor(DbModule dbModule)
            : base(
                "table-info",
                $".{nameof(TableInfoExecutor)}.lisp",
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
