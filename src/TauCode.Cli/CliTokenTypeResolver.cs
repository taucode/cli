using TauCode.Parsing.Tokens;

namespace TauCode.Cli;

public class CliTokenTypeResolver : ILexicalTokenTypeResolver
{
    public virtual Type Resolve(string tokenTypeTag)
    {
        switch (tokenTypeTag)
        {
            case "string":
                return typeof(StringToken);

            case "uri":
                return typeof(UriToken);

            case "file-path":
                return typeof(FilePathToken);

            case "host-name":
                return typeof(HostNameToken);

            case "email-address":
                return typeof(EmailAddressToken);

            case "integer":
                return typeof(Int32Token);

            default:
                throw new NotImplementedException($"error: unknown token type tag: '{tokenTypeTag}'.");
        }
    }
}