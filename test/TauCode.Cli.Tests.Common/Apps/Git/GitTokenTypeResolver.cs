using System;
using TauCode.Cli.Tests.Common.Apps.Git.Tokens;

namespace TauCode.Cli.Tests.Common.Apps.Git
{
    public class GitTokenTypeResolver : CliTokenTypeResolver
    {
        public override Type Resolve(string tokenTypeTag)
        {
            switch (tokenTypeTag)
            {
                case "branch-name":
                    return typeof(BranchNameToken);

                case "ref-name":
                    return typeof(RefNameToken);
            }

            return base.Resolve(tokenTypeTag);
        }
    }
}
