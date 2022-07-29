using TauCode.Cli.Exceptions;

namespace TauCode.Cli.ReplCommandProcessors;

public class ExitProcessor : ReplCommandProcessor
{
    public ExitProcessor(ReplHost host)
        : base(
            host,
            new[]
            {
                "-exit",
            },
            "Exit")
    {
    }

    public override void Process(ReplContext replContext)
    {
        throw new ExitReplException();
    }
}