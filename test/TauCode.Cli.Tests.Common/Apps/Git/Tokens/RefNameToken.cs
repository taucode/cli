using TauCode.Parsing;

namespace TauCode.Cli.Tests.Common.Apps.Git.Tokens
{
    public class RefNameToken : TextTokenBase
    {
        public RefNameToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
