using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common
{
    public class CommandDto
    {
        public string ExecutorName { get; set; }

        public Dictionary<string, List<string>> Arguments { get; set; }

        public Dictionary<string, List<string>> KeyValues { get; set; }

        public List<string> Switches { get; set; }
    }
}
