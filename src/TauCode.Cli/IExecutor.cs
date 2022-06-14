using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public interface IExecutor
    {
        string Name { get; }

        void Execute(IList<ILexicalToken> tokens, IExecutionContext executionContext);

        Task ExecuteAsync(
            IList<ILexicalToken> tokens,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default);
    }
}
