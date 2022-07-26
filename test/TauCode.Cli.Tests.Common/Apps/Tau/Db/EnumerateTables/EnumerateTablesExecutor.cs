using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Exceptions;
using TauCode.Db.SqlClient;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.EnumerateTables
{
    public class EnumerateTablesExecutor : DbExecutor
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
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            IDbConnection connection;
            string connectionString;

            if ((connectionString = command.KeyValues.GetValueOrDefault("connection")?.SingleOrDefault()?.ToString()) != null)
            {
                var dbProvider = command.KeyValues["provider"].Single().ToString().ToEnum<DbProvider>();
                connection = DbHelper.CreateConnection(connectionString, dbProvider);
            }
            else
            {
                var ctx = (DbExecutionContext)executionContext;

                connection =
                    ctx.Connection
                    ??
                    throw new CliException("No connection provided and there is no current execution context.");
            }

            var dbInspector = new SqlInspector((SqlConnection)connection, "dbo");
            var tableNames = dbInspector.GetTableNames();

            foreach (var tableName in tableNames)
            {
                await executionContext.Output.WriteLineAsync(tableName.AsMemory(), cancellationToken);
            }
        }

        protected override void ExecuteImplReal(
            Command command,
            IExecutionContext executionContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
