using TauCode.Parsing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli.Tests.Common
{
    public class AnyTokenNode : ActionNode
    {
        protected override bool AcceptsImpl(ParsingContext parsingContext) => true;

        protected override string GetDataTag()
        {
            return null;
        }
    }
}
