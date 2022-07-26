using Serilog;
using System.IO;

namespace TauCode.Cli
{
    public abstract class ExecutionContextBase : IExecutionContext
    {
        protected ExecutionContextBase(
            ILogger logger,
            TextReader input,
            TextWriter output)
        {
            this.Logger = logger;
            this.Input = input;
            this.Output = output;
        }

        public ILogger Logger { get; }
        public TextReader Input { get; }
        public TextWriter Output { get; }

        public void Dispose()
        {
            this.DisposeImpl();
        }

        protected abstract void DisposeImpl();
    }
}
