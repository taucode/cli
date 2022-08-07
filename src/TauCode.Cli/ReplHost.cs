using Serilog;
using System.Text;
using TauCode.Cli.Exceptions;
using TauCode.Cli.ReplCommandProcessors;

namespace TauCode.Cli;

// todo: disposed app, module, executor and so on cannot 'Run', cannot 'Add', etc.

// todo unused methods
public class ReplHost : ExecutionContextBuilder, IReplHost
{
    #region Fields

    private readonly Dictionary<string, IApp> _apps;

    private Dictionary<string, ReplCommandProcessor>? _replCommandProcessors;

    #endregion

    #region ctor

    public ReplHost(TextReader input, TextWriter output)
    {
        _apps = new Dictionary<string, IApp>();

        this.Input = input ?? throw new ArgumentNullException(nameof(input));
        this.Output = output ?? throw new ArgumentNullException(nameof(output));
    }

    #endregion

    #region Private

    private string FormAppList()
    {
        var sb = new StringBuilder();

        foreach (var app in this.Apps)
        {
            this.WriteApp(app, sb);
        }

        return sb.ToString();
    }

    private void WriteApp(IApp app, StringBuilder sb)
    {
        foreach (var module in app.Modules)
        {
            this.WriteModule(app, module, sb);
        }
    }

    private void WriteModule(IApp app, IModule module, StringBuilder sb)
    {
        foreach (var executor in module.Executors)
        {
            this.WriteExecutor(app, module, executor, sb);
        }
    }

    private void WriteExecutor(IApp app, IModule module, IExecutor executor, StringBuilder sb)
    {
        throw new NotImplementedException();

        //if (app.Name != null)
        //{
        //    sb.Append(app.Name);
        //}

        //if (module.Name != null)
        //{
        //    if (app.Name != null)
        //    {
        //        sb.Append(" ");
        //    }

        //    sb.Append(module.Name);
        //}

        //if (executor.Name != null)
        //{
        //    if (app.Name != null || module.Name != null)
        //    {
        //        sb.Append(" ");
        //    }

        //    sb.Append(executor.Name);
        //}

        //sb.AppendLine();
    }

    #endregion

    #region Protected

    protected virtual string MakePrompt()
    {
        var sb = new StringBuilder();

        if (this.CurrentApp is { Name: { } })
        {
            sb.Append(this.CurrentApp);
            sb.Append(" ");
        }

        if (this.CurrentModule is { Name: { } })
        {
            sb.Append(this.CurrentModule);
            sb.Append(" ");
        }

        if (this.CurrentExecutor is { Name: { } })
        {
            sb.Append(this.CurrentExecutor);
            sb.Append(" ");
        }

        sb.Append("$ ");

        return sb.ToString();
    }

    protected virtual void DisposeImpl()
    {
        // idle
    }

    protected virtual void ShowKeysHelp()
    {
        var help = @"
-u, --use       : Use specific app/module/executor
-cls            : Clear screen
-exit           : Exit application
-e, --enumerate : Enumerate all apps
-c, --context   : Show current context
-h, --help      : This help message
";

        this.Output?.WriteLine(help);
    }

    protected virtual IList<ReplCommandProcessor> CreateCommandProcessors()
    {
        return new List<ReplCommandProcessor>
        {
            new UseProcessor(this),
            new EnumerateProcessor(this),
            new ClearScreenProcessor(this),
            new ExitProcessor(this),
        };
    }

    protected Dictionary<string, ReplCommandProcessor> GetCommandProcessors()
    {
        if (_replCommandProcessors == null)
        {
            _replCommandProcessors = new Dictionary<string, ReplCommandProcessor>();

            var processors = this.CreateCommandProcessors();
            foreach (var processor in processors)
            {
                foreach (var key in processor.Keys)
                {
                    _replCommandProcessors.Add(key, processor);
                }
            }
        }

        return _replCommandProcessors;
    }

    protected virtual bool TryProcessCommand(ReplContext replContext)
    {
        var key = replContext.TryExtractKey();
        if (key == null)
        {
            return false;
        }

        var processor = this.GetCommandProcessors()!.GetValueOrDefault(key);
        if (processor == null)
        {
            replContext.Rewind();

            throw new CliException($"Unknown REPL command: '{key}'");
        }

        processor.Process(replContext);
        return true;
    }

    protected virtual async Task HandleExceptionAsync(Exception ex, CancellationToken cancellationToken)
    {
        await this.Output.WriteLineAsync(ex.Message.AsMemory(), cancellationToken);
    }

    #endregion

    #region Overridden

    public override ExecutionContext BuildFromSelf(ReadOnlyMemory<char> input, bool allowIncomplete)
    {
        input = input.SkipCliWhiteSpaces();

        var result = this.TermExtractor.TryExtract(input.Span, out var appName);
        if (result.ErrorCode.HasValue)
        {
            throw new CliException("App name expected");
        }

        var app = _apps!.GetValueOrDefault(appName);
        if (app == null)
        {
            throw new CliException($"App not found: '{appName}'.");
        }

        var appInput = input[result.CharsConsumed..].SkipCliWhiteSpaces();

        return this.BuildFromApp(app, appInput, allowIncomplete);
    }

    public override ILogger? GetLogger() => this.Logger;

    public override TextReader? GetInput() => this.Input;

    public override TextWriter? GetOutput() => this.Output;

    #endregion

    #region IReplHost Members

    public IReadOnlyList<IApp> Apps => _apps.Values.ToList();

    public void AddApp(IApp app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (!(app is App))
        {
            throw new ArgumentException($"'{app}' must be of type '{typeof(App).FullName}'.");
        }

        if (_apps.ContainsKey(app.Name))
        {
            throw new InvalidOperationException($"App with name '{app.Name}' already added.");
        }

        _apps.Add(app.Name, app);
    }

    public IApp GetApp(string appName)
    {
        if (appName == null)
        {
            throw new ArgumentNullException(nameof(appName));
        }

        return _apps.GetValueOrDefault(appName) ?? throw new CliException($"App '{appName}' not found.");
    }

    public IApp? CurrentApp { get; private set; }

    public IModule? CurrentModule { get; private set; }

    public IExecutor? CurrentExecutor { get; private set; }

    public IReadOnlyList<ReplCommandProcessor> CommandProcessors => this.GetCommandProcessors()
        .Values
        .Distinct() // todo: performance?
        .ToList();

    public ILogger? Logger { get; set; }

    public TextReader? Input { get; set; }

    public TextWriter? Output { get; set; }

    public void Run(string[]? script = null)
    {
        throw new NotImplementedException();
        //while (true)
        //{
        //    var prompt = this.MakePrompt();
        //    this.GetOutput().Write(prompt);
        //    var input = this.GetInput().ReadLine();

        //    throw new NotImplementedException();
        //}
    }

    public async Task RunAsync(
        string[]? script = null,
        CancellationToken cancellationToken = default)
    {
        var scriptQueue = new Queue<string?>(script ?? Array.Empty<string>());
        var replContext = new ReplContext(this);

        while (true)
        {
            var prompt = this.MakePrompt();
            await this.Output.WriteAsync(prompt);

            string? input;
            if (scriptQueue.Count > 0)
            {
                input = scriptQueue.Dequeue();
            }
            else
            {
                input = await this.Input.ReadLineAsync();
            }

            if (input == null)
            {
                continue;
            }

            replContext.Reset(input);

            try
            {
                var processedByRepl = this.TryProcessCommand(replContext);
                if (processedByRepl)
                {
                    continue;
                }

                ExecutionContext executionContext;
                var app = this.CurrentApp;
                var module = this.CurrentModule;
                var executor = this.CurrentExecutor;

                if (executor != null)
                {
                    executionContext = this.BuildFromExecutor(
                        app ?? throw new CliException("Internal error"), // todo: currenet app must not be null here; 'Internal error' is copy-pasted
                        module ?? throw new CliException("Internal error"), // todo: currenet app must not be null here; 'Internal error' is copy-pasted,
                        executor!,
                        replContext.RemainingInput,
                        false);
                }
                else if (module != null)
                {
                    executionContext = this.BuildFromModule(
                        app ?? throw new CliException("Internal error"), // todo: currenet app must not be null here; 'Internal error' is copy-pasted
                        module,
                        replContext.RemainingInput,
                        false);
                }
                else if (app != null)
                {
                    executionContext = this.BuildFromApp(
                        app,
                        replContext.RemainingInput,
                        false);
                }
                else
                {
                    executionContext = this.BuildFromSelf(replContext.RemainingInput, false);
                }

                await executionContext.Executor!.ExecuteAsync(executionContext, cancellationToken);
            }
            catch (ExitReplException)
            {
                break;
            }
            catch (Exception ex)
            {
                await this.HandleExceptionAsync(ex, cancellationToken);
            }
        }
    }

    public void SetCurrentState(IApp? currentApp, IModule? currentModule, IExecutor? currentExecutor)
    {
        if (currentExecutor != null)
        {
            if (currentModule == null)
            {
                throw new NotImplementedException();
            }

            if (!currentModule.Contains(currentExecutor))
            {
                throw new NotImplementedException();
            }
        }

        if (currentModule != null)
        {
            if (currentApp == null)
            {
                throw new NotImplementedException();
            }

            if (!currentApp.Contains(currentModule))
            {
                throw new NotImplementedException();
            }
        }

        if (currentApp != null)
        {
            if (!_apps.ContainsValue(currentApp))
            {
                throw new NotImplementedException();
            }
        }

        this.CurrentApp = currentApp;
        this.CurrentModule = currentModule;
        this.CurrentExecutor = currentExecutor;
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        foreach (var app in _apps.Values)
        {
            app.Dispose();
        }

        this.DisposeImpl();
    }

    #endregion
}
