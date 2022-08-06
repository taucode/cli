using System;
using System.IO;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email.Send;

public class SendTokenConverter : ILexicalTokenConverter
{
    public ILexicalToken Convert(ILexicalToken token, Type otherLexicalTokenType, IParsingResult parsingResult)
    {
        if (
            otherLexicalTokenType == typeof(FilePathToken) &&
            token.GetType().IsIn(
                typeof(HostNameToken)
            )
        )
        {
            var textToken = (TextTokenBase)token;

            if (IsValidFilePath(textToken.Text))
            {
                return new FilePathToken(token.Position, token.ConsumedLength, textToken.Text);
            }
        }

        return null;
    }

    public TOtherLexicalTokenType Convert<TOtherLexicalTokenType>(ILexicalToken token, IParsingResult parsingResult)
        where TOtherLexicalTokenType : class, ILexicalToken
    {
        throw new NotImplementedException();
    }

    private static bool IsValidFilePath(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            return true;
        }
        catch
        {
            return false;
        }
    }
}