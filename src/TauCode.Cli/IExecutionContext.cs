using Serilog;
using System;
using System.IO;

namespace TauCode.Cli
{
    public interface IExecutionContext : IDisposable
    {
        ILogger Logger { get; }
        TextReader Input { get; }
        TextWriter Output { get; }
    }
}
