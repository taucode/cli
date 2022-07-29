using System;
using TauCode.Cli.Tests.Common.Apps.Git.Tokens;
using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tests.Common.Apps.Git;

// todo: base abstract class for ILexicalTokenConverter
public class GitTokenConverter : ILexicalTokenConverter
{
    public ILexicalToken Convert(ILexicalToken token, Type otherLexicalTokenType, IParsingResult parsingResult)
    {
        if (token == null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        if (otherLexicalTokenType == null)
        {
            throw new ArgumentNullException(nameof(otherLexicalTokenType));
        }

        if (token.GetType() == otherLexicalTokenType)
        {
            return token;
        }

        if (token is TextTokenBase textToken)
        {
            if (otherLexicalTokenType == typeof(BranchNameToken))
            {
                if (GitHelper.IsValidBranchName(textToken.Text))
                {
                    return new BranchNameToken(textToken.Position, textToken.ConsumedLength, textToken.Text);
                }
            }
            else if (otherLexicalTokenType == typeof(RefNameToken))
            {
                if (GitHelper.IsValidRefName(textToken.Text))
                {
                    return new RefNameToken(textToken.Position, textToken.ConsumedLength, textToken.Text);
                }
            }
            else if (otherLexicalTokenType == typeof(KeyToken))
            {
                if (CliHelper.IsValidKey(textToken.Text))
                {
                    return new KeyToken(textToken.Position, textToken.ConsumedLength, textToken.Text);
                }
            }
        }

        return null;
    }

    public TOtherLexicalTokenType Convert<TOtherLexicalTokenType>(ILexicalToken token, IParsingResult parsingResult)
        where TOtherLexicalTokenType : class, ILexicalToken
    {
        return (TOtherLexicalTokenType)Convert(token, typeof(TOtherLexicalTokenType), parsingResult);
    }
}