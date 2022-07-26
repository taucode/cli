using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public interface IModule
    {
        string Name { get; }

        ILexer Lexer { get; }

        IReadOnlyList<IExecutor> Executors { get; }

        void AddExecutor(IExecutor executor);

        IExecutor GetExecutor(string executorName);

        IExecutionContext CreateExecutionContext(ILogger logger, TextReader input, TextWriter output);

        IExecutionContext CurrentExecutionContext { get; set; }

        event Action<IModule> CurrentExecutionContextChanged;
    }
}
