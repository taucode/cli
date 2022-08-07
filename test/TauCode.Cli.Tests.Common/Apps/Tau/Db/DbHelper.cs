using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using TauCode.Db;
using TauCode.Db.Npgsql;
using TauCode.Db.SqlClient;
using TauCode.Db.SQLite;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db;

public static class DbHelper
{
    private static readonly CliGraphParser GraphParser = new(
        new CliGraphScriptReader(),
        new CliGraphBuilder(
            new CliVertexFactory(new DbTokenTypeResolver())));

    private static readonly HashSet<char> PunctuationChars = new(new[]
    {
        ',',
        '*',
        ';',
    });

    public static ILexer Lexer = new Lexer
    {
        Producers = new ILexicalTokenProducer[]
        {
            new WhiteSpaceProducer(),
            new JsonStringProducer(CliHelper.IsCliWhiteSpace),
            new KeyProducer(CliHelper.IsCliWhiteSpace),
            new EnumProducer<DbProvider>(true, CliHelper.IsCliWhiteSpace),
            new TermProducer(CliHelper.IsCliWhiteSpace),
            new PunctuationProducer(PunctuationChars, IsPunctuationTerminationChar),
            new WordProducer(CliHelper.IsCliWhiteSpace),
        }
    };

    public static bool IsPunctuationChar(this char c) => PunctuationChars.Contains(c);

    public static bool IsPunctuationTerminationChar(ReadOnlySpan<char> input, int pos)
    {
        if (CliHelper.IsCliWhiteSpace(input, pos))
        {
            return true;
        }

        var c = input[pos];
        if (c.IsPunctuationChar())
        {
            return true;
        }

        return false;
    }

    public static IDbConnection CreateConnection(string connectionString, DbProvider dbProvider)
    {
        IDbConnection connection;

        switch (dbProvider)
        {
            case DbProvider.SqlServer:
                connection = new SqlConnection(connectionString);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        connection.Open();

        return connection;
    }

    public static CliGraph BuildCliGraph(string graphResourceName)
    {
        var script = typeof(DbHelper).Assembly.GetResourceText(graphResourceName, true);
        var graph = GraphParser.ParseScript(script);
        return graph;
    }

    public static IDbUtilityFactory ResolveFactory(IDbConnection connection)
    {
        if (connection is SqlConnection)
        {
            return SqlUtilityFactory.Instance;
        }
        else if (connection is SQLiteConnection)
        {
            return SQLiteUtilityFactory.Instance;
        }
        else if (connection is NpgsqlConnection)
        {
            return NpgsqlUtilityFactory.Instance;
        }

        throw new ArgumentException($"Not supported connection: '{connection.GetType().FullName}'.", nameof(connection));
    }
}