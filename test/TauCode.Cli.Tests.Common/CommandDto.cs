using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common;

public class CommandDto
{
    public string ExecutorName { get; set; } = null!;

    public Dictionary<string, List<string>> Arguments { get; set; } = null!;

    public Dictionary<string, List<string>> KeyValues { get; set; } = null!;

    public List<string> Switches { get; set; } = null!;
}