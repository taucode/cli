using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Data.Graphs;

namespace TauCode.Cli.Tests.Common.Apps
{
    public class TestExecutorBase : ExecutorBase
    {
        protected TestExecutorBase(string name, IGraph parsingGraph)
            : base(name, parsingGraph)
        {
        }

        protected override void ExecuteImpl(Command command, IExecutionContext executionContext)
        {
            var dto = command.ToDto();
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            executionContext.Output.WriteLine("Running sync");
            executionContext.Output.WriteLine(json);
        }

        protected override async Task ExecuteImplAsync(
            Command command,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            var dto = command.ToDto();
            var json = JsonConvert.SerializeObject(dto, Formatting.Indented);
            await executionContext.Output.WriteLineAsync("Running async");
            await executionContext.Output.WriteLineAsync(json.AsMemory(), cancellationToken);
        }
    }
}
