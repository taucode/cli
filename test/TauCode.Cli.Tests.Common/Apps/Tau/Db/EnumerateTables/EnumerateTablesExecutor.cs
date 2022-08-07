using System;
using System.Threading;
using System.Threading.Tasks;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.EnumerateTables;

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

    protected override async Task ExecuteImplRealAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken)
    {
        var connection = this.GetOrCreateConnection(command, out var dbProvider);
        var factory = DbHelper.ResolveFactory(connection);
        var dbInspector = factory.CreateInspector(connection, null!);
        var tableNames = dbInspector.GetTableNames();

        foreach (var tableName in tableNames)
        {
            await executionContext.Output!.WriteLineAsync(tableName.AsMemory(), cancellationToken);
        }
    }
}
