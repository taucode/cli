using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common;

public class TestCommandDto
{
    public string? ExecutorName { get; set; }

    public string Input { get; set; } = null!;

    public Dictionary<string, List<string?>> Arguments { get; set; } = null!;

    public Dictionary<string, List<string?>> KeyValues { get; set; } = null!;

    public List<string> Switches { get; set; } = null!;
}