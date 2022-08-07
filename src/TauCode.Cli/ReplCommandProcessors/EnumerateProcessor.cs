using System.Text;

namespace TauCode.Cli.ReplCommandProcessors;

public class EnumerateProcessor : ReplCommandProcessor
{
    public EnumerateProcessor(
        ReplHost host)
        : base(
            host,
            new string[]
            {
                "-e",
                "--enumerate"
            },
            "Enumerates REPL commands and executors")
    {
    }

    public override void Process(ReplContext replContext)
    {
        var sb = new StringBuilder();

        foreach (var commandProcessor in this.Host.CommandProcessors)
        {
            for (var i = 0; i < commandProcessor.Keys.Count; i++)
            {
                var key = commandProcessor.Keys[i];
                sb.Append(key);
                if (i < commandProcessor.Keys.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(" : ");
            sb.Append(commandProcessor.Description);
            sb.AppendLine();
        }

        this.Host.Output.Write(sb.ToString());
    }
}