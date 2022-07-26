using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Db
{
    public class DbTokenTypeResolver : CliTokenTypeResolver
    {
        public override Type Resolve(string tokenTypeTag)
        {
            if (tokenTypeTag == "db-provider")
            {
                return typeof(EnumToken<DbProvider>);
            }

            return base.Resolve(tokenTypeTag);
        }
    }
}
