using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.DropTables
{
    public class DropTablesExecutor : DbExecutor
    {
        public DropTablesExecutor(DbModule dbModule)
            : base(
                "drop-all-tables",
                $".{nameof(DropTablesExecutor)}.lisp",
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
