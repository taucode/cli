using Newtonsoft.Json;
using System.IO;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common;

// todo clean
public static class Helper
{
    //private static readonly CliGraphScriptReader ScriptReader =
    //    new CliGraphScriptReader(new CliVertexFactory(new CliTokenTypeResolver()));


    //internal static IGraph BuildParsingGraph(string resourceName)
    //{
    //    var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
    //    var graph = ScriptReader.BuildGraph(script);
    //    return graph;
    //}

    //internal static IParsingNode BuildParsingNode(string resourceName)
    //{
    //    var script = typeof(Helper).Assembly.GetResourceText(resourceName, true);
    //    var node = ScriptReader.BuildNode(script);
    //    return node;
    //}

    private static readonly CliGraphParser GraphParser = new CliGraphParser(
        new CliGraphScriptReader(),
        new CliGraphBuilder(
            new CliVertexFactory(
                new CliTokenTypeResolver())));

    //public static TestCommandDto ToDto(this Command command)
    //{
    //    return new TestCommandDto
    //    {
    //        ExecutorName = command.ExecutorName,

    //        Arguments = command.Arguments
    //            .ToDictionary(
    //                x => x.Key,
    //                y => y.Value
    //                    .Select(x => x.ToString())
    //                    .ToList()),

    //        KeyValues = command.KeyValues
    //            .ToDictionary(
    //                x => x.Key,
    //                y => y.Value
    //                    .Select(x => x.ToString())
    //                    .ToList()),

    //        Switches = command.Switches
    //            .ToList(),
    //    };
    //}

    internal static void Dump(TestCommand command, TextWriter output)
    {
        var commandDto = command.ToDto();
        var json = JsonConvert.SerializeObject(commandDto, Formatting.Indented);
        output.WriteLine(json);
    }

    public static CliGraph BuildCliGraph(string graphResourceName)
    {
        var script = typeof(Helper).Assembly.GetResourceText(graphResourceName, true);
        var graph = GraphParser.ParseScript(script);
        return graph;
    }
}
