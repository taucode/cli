using System.Linq;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing;

namespace TauCode.Cli.Tests.Common
{
    public static class Helper
    {
        private static readonly CliGraphScriptReader ScriptReader =
            new CliGraphScriptReader(new CliVertexFactory(new CliTokenTypeResolver()));


        internal static IGraph BuildParsingGraph(string resourceName)
        {
            var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
            var graph = ScriptReader.BuildGraph(script);
            return graph;
        }

        internal static IParsingNode BuildParsingNode(string resourceName)
        {
            var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
            var node = ScriptReader.BuildNode(script);
            return node;
        }

        public static CommandDto ToDto(this Command command)
        {
            return new CommandDto
            {
                ExecutorName = command.ExecutorName,

                Arguments = command.Arguments
                    .ToDictionary(
                        x => x.Key,
                        y => y.Value
                            .Select(x => x.ToString())
                            .ToList()),

                KeyValues = command.KeyValues
                    .ToDictionary(
                        x => x.Key,
                        y => y.Value
                            .Select(x => x.ToString())
                            .ToList()),

                Switches = command.Switches
                    .ToList(),
            };
        }
    }
}
