namespace TauCode.Cli;

public interface ILexicalTokenTypeResolver
{
    Type Resolve(string tokenTypeTag);
}