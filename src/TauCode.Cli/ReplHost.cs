using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Exceptions;
using TauCode.Data.Text.TextDataExtractors;

namespace TauCode.Cli
{
    public class ReplHost : IReplHost
    {
        #region Fields

        private readonly KeyExtractor _keyExtractor;
        private readonly TermExtractor _termExtractor;

        private readonly Dictionary<string, IApp> _apps;
        private string _prompt;

        // todo clean
        //private readonly ILexer _replLexer;

        #endregion

        #region ctor

        public ReplHost()
        {
            _apps = new Dictionary<string, IApp>();

            _keyExtractor = new KeyExtractor(CliHelper.IsCliWhiteSpace);
            _termExtractor = new TermExtractor(CliHelper.IsCliWhiteSpace);

            //_replLexer = new Lexer();

            //_replLexer.Producers = new ILexicalTokenProducer[]
            //{
            //    new WhiteSpaceProducer(),
            //    new KeyProducer(ReplTerminator),
            //    new TermProducer(ReplTerminator),
            //    new ReplArgProducer(ReplTerminator),
            //};
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
            if (app.Name != null)
            {
                sb.Append(app.Name);
            }

            if (module.Name != null)
            {
                if (app.Name != null)
                {
                    sb.Append(" ");
                }

                sb.Append(module.Name);
            }

            if (executor.Name != null)
            {
                if (app.Name != null || module.Name != null)
                {
                    sb.Append(" ");
                }

                sb.Append(executor.Name);
            }

            sb.AppendLine();
        }

        #endregion

        #region Protected

        protected virtual string MakePrompt()
        {
            if (_prompt == null)
            {
                var sb = new StringBuilder();

                if (this.CurrentApp is { Name: { } })
                {
                    sb.Append(this.CurrentApp.Name);
                    sb.Append(" ");
                }

                if (this.CurrentModule is { Name: { } })
                {
                    sb.Append(this.CurrentModule.Name);
                    sb.Append(" ");

                    if (this.CurrentModule.CurrentExecutionContext != null)
                    {
                        sb.Append("[*] ");
                    }
                }

                if (this.CurrentExecutor is { Name: { } })
                {
                    sb.Append(this.CurrentExecutor.Name);
                    sb.Append(" ");
                }

                sb.Append("$ ");

                _prompt = sb.ToString();
            }

            return _prompt;
        }

        protected TextReader GetInput() => this.Input ?? throw new CliException("Output is null.");

        protected TextWriter GetOutput() => this.Output ?? throw new CliException("Output is null.");

        protected static ReadOnlySpan<char> SkipWhiteSpace(ReadOnlySpan<char> input)
        {
            var pos = 0;
            while (pos < input.Length && CliHelper.IsCliWhiteSpace(input, pos))
            {
                pos++;
            }

            var result = input[pos..];
            return result;
        }

        protected bool TryProcessReplCommand(ReplContext replContext, out string key)
        {
            key = replContext.TryExtractKey();

            if (key == null)
            {
                return false;
            }

            return this.DispatchReplCommand(key, replContext);
        }

        protected virtual bool DispatchReplCommand(string value, ReplContext replContext)
        {
            switch (value)
            {
                case "-u":
                case "--use":
                    this.Use(replContext);
                    return true;

                case "-cls":
                    this.ClearScreen();
                    return true;

                case "-exit":
                    this.Exit();
                    return true;

                case "-c":
                case "--context":
                    this.ShowContext();
                    return true;

                case "-h":
                case "--help":
                    this.ShowKeysHelp();
                    return true;

                default:
                    return false;
            }
        }

        protected virtual void ShowContext()
        {
            if (this.CurrentModule == null)
            {
                this.GetOutput().WriteLine("Specify current module to show context (command: -u/--use)");
            }
            else if (this.CurrentModule.CurrentExecutionContext == null)
            {
                this.GetOutput().WriteLine("Current module has no current execution context.");
            }
            else
            {
                var description = this.CetContextDescription(this.CurrentModule.CurrentExecutionContext);
                this.GetOutput().WriteLine(description);
            }
        }

        protected virtual string CetContextDescription(IExecutionContext executionContext)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            return executionContext.ToString();
        }

        protected void Use(ReplContext replContext)
        {
            _prompt = null;

            if (replContext.RemainingInput.Length == 0)
            {
                this.CurrentApp = null;
                this.CurrentModule = null;
                this.CurrentExecutor = null;

                return;
            }

            this.ResolveReplContext(
                null,
                null,
                null,
                false,
                replContext);

            this.CurrentApp = replContext.App;
            this.CurrentModule = replContext.Module;
            this.CurrentExecutor = replContext.Executor;

            if (this.CurrentModule != null)
            {
                this.CurrentModule.CurrentExecutionContext = replContext.ExecutionContext;
            }
        }

        protected virtual void ClearScreen()
        {
            if (this.Output == null)
            {
                return;
            }

            if (this.Output == Console.Out)
            {
                Console.Clear();
                return;
            }

            throw new NotSupportedException($"Cannot clear output of type '{this.Output.GetType().FullName}'.");
        }

        protected virtual void Exit()
        {
            throw new ExitReplException();
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

        protected void ResolveReplContext(
            IApp app,
            IModule module,
            IExecutor executor,
            bool moduleAndExecutorMustBeResolved,
            ReplContext replContext)
        {
            do
            {
                if (executor == null)
                {
                    string term;
                    if (module == null)
                    {
                        if (app == null)
                        {
                            term = replContext.TryExtractTerm();
                            if (term == null)
                            {
                                throw new CliException($"App name expected'.");
                            }

                            app = this.GetApp(term);
                            if (app == null)
                            {
                                throw new CliException($"App '{term}' not found.");
                            }
                        }

                        // try to get nameless module
                        module = app.GetModule(null);

                        if (module == null)
                        {
                            // try to get named module
                            term = replContext.TryExtractTerm();
                            if (term == null)
                            {
                                if (moduleAndExecutorMustBeResolved)
                                {
                                    throw new CliException($"Module name expected for app '{app.Name}'.");
                                }
                                else
                                {
                                    break;
                                }
                            }

                            module = app.GetModule(term);
                            if (module == null)
                            {
                                throw new CliException($"Module '{term}' not found in app '{app.Name}'.");
                            }
                        }
                    }

                    // try to gen nameless executor
                    executor = module.GetExecutor(null);

                    if (executor == null)
                    {
                        // try to get named executor
                        term = replContext.TryExtractTerm();
                        if (term == null)
                        {
                            if (moduleAndExecutorMustBeResolved)
                            {
                                throw new CliException($"Executor name expected for app '{app.Name}', module '{module.Name}'.");
                            }
                            else
                            {
                                break;
                            }
                        }

                        executor = module.GetExecutor(term);
                        if (executor == null)
                        {
                            throw new CliException($"Executor '{term}' not found in app '{app.Name}', module '{module.Name}'.");
                        }
                    }
                }
            } while (false);

            replContext.App = app;
            replContext.Module = module;
            replContext.Executor = executor;
            replContext.ExecutionContext = module?.CurrentExecutionContext;
        }

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

            foreach (var module in app.Modules)
            {
                module.CurrentExecutionContextChanged += Module_CurrentExecutionContextChanged;
            }
        }

        private void Module_CurrentExecutionContextChanged(IModule module)
        {
            _prompt = null;
        }

        public IApp GetApp(string appName)
        {
            if (appName == null)
            {
                throw new ArgumentNullException(nameof(appName));
            }

            return _apps.GetValueOrDefault(appName) ?? throw new CliException($"App '{appName}' not found.");
        }

        public IApp CurrentApp { get; private set; }

        public IModule CurrentModule { get; private set; }

        public IExecutor CurrentExecutor { get; private set; }

        public ILogger Logger { get; set; }

        public TextReader Input { get; set; }

        public TextWriter Output { get; set; }

        public void Run()
        {
            while (true)
            {
                var prompt = this.MakePrompt();
                this.GetOutput().Write(prompt);
                var input = this.GetInput().ReadLine();

                throw new NotImplementedException();
            }
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            var replContext = new ReplContext();

            // todo clean
            var todoIdx = 0;

            while (true)
            {
                var prompt = this.MakePrompt();
                await this.GetOutput().WriteAsync(prompt);

                string inputText;
                if (todoIdx == 0)
                {
                    inputText = "tau db connect -c \"Server=.;Database=EZFin.Taxonomies;Trusted_Connection=True;TrustServerCertificate=True\" -p sqlserver";
                    await this.GetOutput().WriteLineAsync(inputText);
                    todoIdx++;
                }
                else if (todoIdx == 1)
                {
                    inputText = "-u tau db query";
                    await this.GetOutput().WriteLineAsync(inputText);
                    todoIdx++;
                }
                else if (todoIdx == 2)
                {
                    inputText = "select * from ez_TaxonomyTypes";
                    await this.GetOutput().WriteLineAsync(inputText);
                    todoIdx++;
                }
                else
                {
                    inputText = await this.GetInput().ReadLineAsync();
                }

                inputText = inputText?.Trim();

                replContext.Reset(inputText);

                if (string.IsNullOrWhiteSpace(inputText))
                {
                    continue;
                }

                string key = null;
                try
                {
                    // is it a key?
                    var replCommandProcessed = this.TryProcessReplCommand(replContext, out key);
                    if (replCommandProcessed)
                    {
                        // nothing else to do.
                    }
                    else
                    {
                        replContext.ResetPosition();

                        this.ResolveReplContext(
                            this.CurrentApp,
                            this.CurrentModule,
                            this.CurrentExecutor,
                            true,
                            replContext);

                        var tokens = replContext.Module.Lexer.Tokenize(replContext.RemainingInput);
                        var module = replContext.Module;
                        var executor = replContext.Executor;
                        var executionContext = replContext.ExecutionContext;
                        if (executionContext == null)
                        {
                            executionContext = module.CreateExecutionContext(
                                this.Logger,
                                this.Input,
                                this.Output);
                        }

                        await executor.ExecuteAsync(
                            tokens,
                            executionContext,
                            cancellationToken);
                    }
                }
                catch (ExitReplException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    await this.Output.WriteLineAsync(ex.Message);

                    if (key != null)
                    {
                        this.ShowKeysHelp();
                    }
                }
            }
        }

        #endregion
    }
}
