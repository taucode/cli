using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db
{
    public static class DbHelper
    {
        private static readonly CliGraphScriptReader ScriptReader =
            new CliGraphScriptReader(new DbVertexFactory());

        private static readonly HashSet<char> PunctuationChars = new HashSet<char>(new[]
        {
            ',',
            '*',
            ';',
        });

        public static IGraph BuildParsingGraph(string resourceName)
        {
            var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
            var graph = ScriptReader.BuildGraph(script);

            return graph;
        }

        public static IParsingNode BuildParsingNode(string resourceName)
        {
            var graph = BuildParsingGraph(resourceName);
            var node = ScriptReader.ResolveParsingNode(graph);

            return node;
        }

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
    }
}
