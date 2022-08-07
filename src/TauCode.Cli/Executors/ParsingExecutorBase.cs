using TauCode.Data.Graphs;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing;

namespace TauCode.Cli.Executors;

public abstract class ParsingExecutorBase : IExecutor
{
    #region Fields

    private static readonly TermExtractor NameExtractor = new();

    private readonly string? _name;

    #endregion

    #region ctor

    protected ParsingExecutorBase(
        string? name,
        ILexer lexer,
        CliGraph graph)
    {
        if (name != null)
        {
            var isValidName = NameExtractor.TryParse(name, out var dummy);
            if (!isValidName)
            {
                throw new ArgumentException("Executor name must be a valid term or <null>.", nameof(name));
            }
        }

        this.Name = name;
        this.Lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));

        this.Graph = graph ?? throw new ArgumentNullException(nameof(graph));

        Parser = new Parser
        {
            Root = graph.RootNode,
        };
    }

    #endregion

    #region Public

    public ILexer Lexer { get; }

    public IGraph Graph { get; }

    public Action<Command, ExecutionContext>? OnBeforeCommandExecuted { get; set; }

    #endregion

    #region Protected

    protected IParser Parser { get; }

    protected virtual Command CreateCommand(string? executorName, ReadOnlyMemory<char> input) => new(executorName, input);

    protected Command ParseCommand(ExecutionContext executionContext)
    {
        var tokens = this.Lexer.Tokenize(executionContext.Arguments);

        this.Parser.Logger = executionContext.Logger;
        var command = this.CreateCommand(this.Name, executionContext.Arguments);

        this.Parser.Parse(tokens, command);

        return command;
    }

    #endregion

    #region Abstract

    protected abstract void ExecuteImpl(
        Command command,
        ExecutionContext executionContext);

    protected abstract Task ExecuteImplAsync(
        Command command,
        ExecutionContext executionContext,
        CancellationToken cancellationToken = default);

    #endregion

    #region Overridden

    public override string? ToString() => this.Name;

    #endregion

    #region IExecutor Members

    public string? Name { get; }

    public void Execute(ExecutionContext executionContext)
    {
        var command = this.ParseCommand(executionContext);
        this.OnBeforeCommandExecuted?.Invoke(command, executionContext);

        this.ExecuteImpl(command, executionContext);
    }

    public async Task ExecuteAsync(
        ExecutionContext executionContext,
        CancellationToken cancellationToken = default)
    {
        var command = this.ParseCommand(executionContext);
        this.OnBeforeCommandExecuted?.Invoke(command, executionContext);

        await this.ExecuteImplAsync(command, executionContext, cancellationToken);
    }

    #endregion

    #region IDisposable Members

    public virtual void Dispose()
    {
        // idle
    }

    #endregion
}
