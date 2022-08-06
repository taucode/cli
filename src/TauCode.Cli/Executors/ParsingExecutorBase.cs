using TauCode.Cli.Exceptions;
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

    protected virtual Command CreateCommand(string? executorName) => new(executorName);

    #endregion

    #region Abstract

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

    public void Execute(
        ExecutionContext executionContext)
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(
        ExecutionContext executionContext,
        CancellationToken cancellationToken = default)
    {
        IList<ILexicalToken> tokens;

        if (executionContext.RawArguments.HasValue)
        {
            tokens = this.Lexer.Tokenize(executionContext.RawArguments.Value);
        }
        else if (executionContext.Arguments != null)
        {
            tokens = this.Lexer.Tokenize(executionContext.Arguments);
        }
        else
        {
            throw new CliException("Either RawArguments or Arguments must have value.");
        }

        this.Parser.Logger = executionContext.Logger;
        var command = this.CreateCommand(this.Name);

        this.Parser.Parse(tokens, command);

        this.OnBeforeCommandExecuted?.Invoke(command, executionContext);
        await this.ExecuteImplAsync(command, executionContext, cancellationToken);
    }

    #endregion
}
