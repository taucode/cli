using System.Data;
using System.Text;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.Connect;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.Disconnect;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.DropTables;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.EnumerateTables;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.PurgeData;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.Query;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeData;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.SerializeMetadata;
using TauCode.Cli.Tests.Common.Apps.Tau.Db.TableInfo;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db;

public class DbModule : Module
{
    public DbModule(bool isReal)
        : base("db")
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

    public IDbConnection? Connection { get; set; }

    public override string? ToString()
    {
        var sb = new StringBuilder();
        sb.Append(this.Name);

        if (this.Connection != null)
        {
            sb.Append($"[{this.Connection.Database}]");
        }

        return sb.ToString();
    }
}