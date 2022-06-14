using System.Collections.Generic;
using TauCode.Parsing;

// todo regions
namespace TauCode.Cli
{
    public class Command : IParsingResult
    {
        public Command(string executorName)
        {
            this.ExecutorName = executorName;
        }

        public string ExecutorName { get; }

        public Dictionary<string, List<ILexicalToken>> Arguments { get; } =
            new Dictionary<string, List<ILexicalToken>>();

        public Dictionary<string, List<ILexicalToken>> KeyValues { get; } =
            new Dictionary<string, List<ILexicalToken>>();

        public HashSet<string> Switches { get; } =
            new HashSet<string>();

        #region IParsingResult Members

        public int Version { get; private set; }

        public void IncreaseVersion()
        {
            this.Version++;
        }

        #endregion
    }
}
