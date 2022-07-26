using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Data.Graphs;
using TauCode.Parsing;

namespace TauCode.Cli
{
    public abstract class ExecutorBase : IExecutor
    {
        #region Fields

        protected readonly IParser Parser;

        #endregion

        #region ctor

        protected ExecutorBase(string name, IGraph parsingGraph, Func<IGraph, IParsingNode> rootResolver)
        {
            // todo checks

            this.Name = name;
            this.Graph = parsingGraph;

            var root = rootResolver(parsingGraph);

            if (this.Graph.Contains(root))
            {
                // ok
            }
            else
            {
                throw new NotImplementedException("error: must belong");
            }

            this.Root = root ?? throw new ArgumentNullException(nameof(root));
            Parser = new Parser
            {
                Root = root,
            };
        }

        protected ExecutorBase(string name, IGraph parsingGraph)
            : this(name, parsingGraph, g => (IParsingNode)g.Single(x => x.Name == "parsing-top"))
        {
        }

        #endregion

        #region Protected

        protected IGraph Graph { get; }

        protected IParsingNode Root { get; }

        protected abstract void ExecuteImpl(Command command, IExecutionContext executionContext);

        protected abstract Task ExecuteImplAsync(
            Command command,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default);

        protected virtual Command CreateCommand() => new Command(this.Name);

        #endregion

        #region Public

        public Action<Command, IExecutionContext> OnBeforeCommandExecuted { get; set; }

        #endregion

        #region IExecutor Members

        public string Name { get; }

        public void Execute(IList<ILexicalToken> tokens, IExecutionContext executionContext)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.Parser.Logger = executionContext.Logger;

            var command = this.CreateCommand();
            this.Parser.Parse(tokens, command);

            this.OnBeforeCommandExecuted?.Invoke(command, executionContext);

            this.ExecuteImpl(command, executionContext);
        }

        public async Task ExecuteAsync(
            IList<ILexicalToken> tokens,
            IExecutionContext executionContext,
            CancellationToken cancellationToken = default)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.Parser.Logger = executionContext?.Logger;

            var command = this.CreateCommand();
            this.Parser.Parse(tokens, command);

            this.OnBeforeCommandExecuted?.Invoke(command, executionContext);

            await this.ExecuteImplAsync(command, executionContext, cancellationToken);
        }

        #endregion
    }
}
