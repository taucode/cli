using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Exceptions;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db;

public abstract class DbExecutor : TestExecutorBase
{
    protected DbExecutor(
        string name,
        string graphResourceName,
        DbModule dbModule)
        : base(
            name,
            DbHelper.Lexer,
            DbHelper.BuildCliGraph(graphResourceName))
    {
        this.DbModule = dbModule;
    }

    public DbModule DbModule { get; }

    protected IDbConnection GetOrCreateConnection(Command command, out DbProvider? dbProvider)
    {
        IDbConnection? connection;

        var connectionString = command.KeyValues.GetValueOrDefault("connection")?.SingleOrDefault()?.ToString();
        dbProvider = command.KeyValues["provider"].SingleOrDefault()?.ToString()?.ToEnum<DbProvider>();

        if (connectionString == null || dbProvider == null)
        {
            connection = this.DbModule.Connection;
        }
        else
        {
            connection = DbHelper.CreateConnection(connectionString, dbProvider.Value);
        }

        if (connection == null)
        {
            throw new CliException("No valid connection specified, and module connection is null.");
        }

        return connection;
    }

    protected override async Task ExecuteImplAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken = default)
    {
        if (this.DbModule.IsReal)
        {
            await this.ExecuteImplRealAsync(
                command,
                executionContext,
                cancellationToken);
        }
        else
        {
            await base.ExecuteImplAsync(
                command,
                executionContext,
                cancellationToken);
        }
    }

    protected abstract Task ExecuteImplRealAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken);
}
