using System;
using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.EnumerateTables;

// todo clean
public class
    EnumerateTablesExecutor : DbExecutor
{
    public EnumerateTablesExecutor(DbModule dbModule)
        : base(
            "enumerate-tables",
            $".{nameof(EnumerateTablesExecutor)}.lisp",
            dbModule)
    {
    }

    //protected override async Task ExecuteImplRealAsync(
    //    Command command,
    //    IExecutionContext executionContext,
    //    CancellationToken cancellationToken = default)
    //{
    //    IDbConnection connection;
    //    string connectionString;

    //    if ((connectionString = command.KeyValues.GetValueOrDefault("connection")?.SingleOrDefault()?.ToString()) != null)
    //    {
    //        var dbProvider = command.KeyValues["provider"].Single().ToString().ToEnum<DbProvider>();
    //        connection = DbHelper.CreateConnection(connectionString, dbProvider);
    //    }
    //    else
    //    {
    //        var ctx = (DbExecutionContext)executionContext;

    //        connection =
    //            ctx.Connection
    //            ??
    //            throw new CliException("No connection provided and there is no current execution context.");
    //    }

    //    var dbInspector = new SqlInspector((SqlConnection)connection, "dbo");
    //    var tableNames = dbInspector.GetTableNames();

    //    foreach (var tableName in tableNames)
    //    {
    //        await executionContext.Output.WriteLineAsync(tableName.AsMemory(), cancellationToken);
    //    }
    //}

    //protected override void ExecuteImplReal(
    //    Command command,
    //    IExecutionContext executionContext)
    //{
    //    throw new System.NotImplementedException();
    //}

    protected override async Task ExecuteImplRealAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        var connection = this.GetOrCreateConnection(command, out var dbProvider);
        var factory = DbHelper.ResolveFactory(connection);
        var dbInspector = factory.CreateInspector(connection, null);
        var tableNames = dbInspector.GetTableNames();

        foreach (var tableName in tableNames)
        {
            await executionContext.Output!.WriteLineAsync(tableName.AsMemory(), cancellationToken);
        }
    }
}
