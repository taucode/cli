using TauCode.Parsing;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Cli.Tests.Common.Apps.Tau.Email;

public static class EmailHelper
{
    // tau email send
    // -h smtp.google.com -p 443 -u joe@example.com -p "psw21!" --subject "hello!" --message "enjoy your CLI!" -a 'c:\\temp\\my-file.jpg' the-file.jpg true -a c:\temp\book.pdf false --to ak@mail.com --to manu@winter.net --cc kek@ta.net --bcc 'ol@ya.com'

    public static ILexer Lexer = new Lexer
    {
        Producers = new ILexicalTokenProducer[]
        {
            new WhiteSpaceProducer(),
            new BooleanProducer(CliHelper.IsCliWhiteSpace),
            new KeyProducer(CliHelper.IsCliWhiteSpace),
            new Int32Producer(CliHelper.IsCliWhiteSpace),
            new HostNameProducer(CliHelper.IsCliWhiteSpace),
            new EmailAddressProducer(CliHelper.IsCliWhiteSpace),
            new JsonStringProducer(CliHelper.IsCliWhiteSpace),
            new FilePathProducer(CliHelper.IsCliWhiteSpace),
        }
    };
}