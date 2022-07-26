using System.Collections.Generic;
using TauCode.Parsing;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db.Query
{
    public class QueryCommand : Command
    {
        public QueryCommand(string executorName)
            : base(executorName)
        {
        }

        public readonly IList<ILexicalToken> Tokens = new List<ILexicalToken>();
    }
}
