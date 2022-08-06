namespace TauCode.Cli.ReplCommandProcessors
{
    public class ClearScreenProcessor : ReplCommandProcessor
    {
        public ClearScreenProcessor(ReplHost host)
            : base(
                host,
                new string[]
                {
                    "-cls",
                },
                "Clear screen")
        {
        }

        public override void Process(ReplContext replContext)
        {
            if (this.Host.Output == Console.Out)
            {
                Console.Clear();
            }
        }
    }
}
