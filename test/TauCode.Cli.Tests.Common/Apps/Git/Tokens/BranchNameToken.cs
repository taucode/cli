using TauCode.Parsing;

namespace TauCode.Cli.Tests.Common.Apps.Git.Tokens
{
    public class BranchNameToken : TextTokenBase
    {
        public BranchNameToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
