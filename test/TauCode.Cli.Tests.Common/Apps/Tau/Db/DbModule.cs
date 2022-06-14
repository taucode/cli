using Serilog;
using System.IO;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.Connect;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.Disconnect;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.DropTables;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.EnumerateTables;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.PurgeData;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.Query;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeData;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeMetadata;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.TableInfo;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db
{
    public class DbModule : Module
    {
        public DbModule(bool isReal)
            : base("db", DbHelper.Lexer)
        {
            this.AddExecutors(
                new ConnectExecutor(this),
                new DisconnectExecutor(this),
                new DropTablesExecutor(this),
                new EnumerateTablesExecutor(this),
                new PurgeDataExecutor(this),
                new QueryExecutor(this),
                new SerializeDataDataExecutor(this),
                new SerializeMetadataExecutor(this),
                new TableInfoExecutor(this));

            this.IsReal = isReal;
        }

        public bool IsReal { get; }

        public override IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output)
        {
            return new DbExecutionContext(logger, input, output, null);
        }
    }
}
