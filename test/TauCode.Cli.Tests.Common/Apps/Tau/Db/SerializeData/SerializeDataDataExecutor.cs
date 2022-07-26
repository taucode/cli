using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeData
{
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
