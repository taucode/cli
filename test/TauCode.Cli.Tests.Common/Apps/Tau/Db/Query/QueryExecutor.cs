using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Executors;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.Query;

// todo clean
public class QueryExecutor : ExecutorBase
{
    public QueryExecutor(DbModule dbModule)
        : base("query")
    {
        //var node = (ActionNode)this.Graph.Single(x => x.Name == "any-token-collector");
        //node.Action = (n, context) =>
        //{
        //    var token = context.GetCurrentToken();
        //    var command = (QueryCommand)context.ParsingResult;
        //    command.Tokens.Add(token);

        //    context.ParsingResult.IncreaseVersion();
        //};

        this.DbModule = dbModule;
    }

    public DbModule DbModule { get; }

    //protected override Command CreateCommand()
    //{
    //    return new QueryCommand(this.Name);
    //}

    //protected override Task ExecuteImplRealAsync(
    //    Command command,
    //    IExecutionContext executionContext,
    //    CancellationToken cancellationToken = default)
    //{
    //    var queryCommand = (QueryCommand)command;
    //    var ctx = (DbExecutionContext)executionContext;
    //    if (ctx.Connection == null)
    //    {
    //        throw new CliException("No connection opened.");
    //    }

    //    using var sqlCommand = ctx.Connection.CreateCommand();
    //    sqlCommand.CommandText = string.Join(" ", queryCommand.Tokens);

    //    using var reader = sqlCommand.ExecuteReader();
    //    var fieldCount = reader.FieldCount;
    //    var fieldNames = Enumerable
    //        .Range(0, fieldCount)
    //        .Select(x => reader.GetName(x))
    //        .ToList();
    //    var values = new object[fieldCount];

    //    var rows = new List<Dictionary<string, object>>();

    //    while (reader.Read())
    //    {
    //        var row = new Dictionary<string, object>();
    //        for (var i = 0; i < fieldCount; i++)
    //        {
    //            var fieldName = reader.GetName(i);
    //            var value = reader.GetValue(i);

    //            row.Add(fieldName, value);
    //        }

    //        rows.Add(row);
    //    }

    //    var rowsJson = JsonConvert.SerializeObject(rows, Formatting.Indented);
    //    ctx.Output.WriteLine(rowsJson);

    //    return Task.CompletedTask;
    //}

    //protected override void ExecuteImplReal(
    //    Command command,
    //    IExecutionContext executionContext)
    //{
    //    throw new System.NotImplementedException();
    //}

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
        sqlCommand.CommandText = executionContext.RawArguments!.Value.ToString(); // todo will throw in real tau.exe

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