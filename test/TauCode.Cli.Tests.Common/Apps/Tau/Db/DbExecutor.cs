using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db
{
    public abstract class DbExecutor : TestExecutorBase
    {
        protected DbExecutor(
            string name,
            string graphResourceName,
            DbModule dbModule)
            : base(
                name,
                DbHelper.BuildParsingGraph(graphResourceName))
        {
            this.DbModule = dbModule;
        }

        public DbModule DbModule { get; }

        protected abstract Task ExecuteImplRealAsync(
            Command command,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default);

        protected abstract void ExecuteImplReal(
            Command command,
            IExecutionContext executionContext);

        protected override async Task ExecuteImplAsync(
            Command command,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            await base.ExecuteImplAsync(command, executionContext, cancellationToken);

            if (this.DbModule.IsReal)
            {
                await this.ExecuteImplRealAsync(command, executionContext, cancellationToken);
            }
        }

        protected override void ExecuteImpl(
            Command command,
            IExecutionContext executionContext)
        {
            base.ExecuteImpl(command, executionContext);

            if (this.DbModule.IsReal)
            {
                this.ExecuteImplReal(command, executionContext);
            }
        }
    }
}
