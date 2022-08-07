using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Executors;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.Query;

public class QueryExecutor : ExecutorBase
{
    public QueryExecutor(DbModule dbModule)
        : base("query")
    {
        this.DbModule = dbModule;
    }

    public DbModule DbModule { get; }

    public override void Execute(ExecutionContext executionContext)
    {
        throw new System.NotImplementedException();
    }

    public override Task ExecuteAsync(ExecutionContext executionContext, CancellationToken cancellationToken = default)
    {
        var connection = this.DbModule.Connection;
        if (connection == null)
        {
            throw new NotImplementedException("error");
        }

        using var sqlCommand = connection.CreateCommand();
        sqlCommand.CommandText = executionContext.Arguments.ToString(); // todo will throw in real tau.exe

        using var reader = sqlCommand.ExecuteReader();
        var fieldCount = reader.FieldCount;
        var fieldNames = Enumerable
            .Range(0, fieldCount)
            .Select(x => reader.GetName(x))
            .ToList();
        var values = new object[fieldCount];

        var rows = new List<Dictionary<string, object>>();

        while (reader.Read())
        {
            var row = new Dictionary<string, object>();
            for (var i = 0; i < fieldCount; i++)
            {
                var fieldName = reader.GetName(i);
                var value = reader.GetValue(i);

                row.Add(fieldName, value);
            }

            rows.Add(row);
        }

        var rowsJson = JsonConvert.SerializeObject(rows, Formatting.Indented);
        executionContext.Output!.WriteLine(rowsJson);

        return Task.CompletedTask;
    }
}