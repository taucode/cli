using System;
using System.Linq;

namespace TauCode.Cli.Tests.Common;

public class TestCommand : Command
{
    public TestCommand(string? executorName, ReadOnlyMemory<char> input)
        : base(executorName, input)
    {
    }

    public virtual TestCommandDto ToDto()
    {
        var dto = new TestCommandDto
        {
            ExecutorName = this.ExecutorName,
            Input = this.Input.ToString(),
            Arguments = this.Arguments.ToDictionary(
                x => x.Key,
                x => x
                    .Value
                    .Select(y => y.ToString())
                    .ToList()),
            KeyValues = this.KeyValues.ToDictionary(
                x => x.Key,
                x => x
                    .Value
                    .Select(y => y.ToString())
                    .ToList()),
            Switches = this.Switches.ToList(),
        };

        return dto;
    }
}
