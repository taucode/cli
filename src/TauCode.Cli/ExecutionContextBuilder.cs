using Serilog;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Executors;
using TauCode.Data.Text.TextDataExtractors;

namespace TauCode.Cli;

public abstract class ExecutionContextBuilder : IExecutionContextBuilder
{
    #region Fields

    protected readonly TermExtractor TermExtractor;

    #endregion

    #region ctor

    public ExecutionContextBuilder()
    {
        TermExtractor = new TermExtractor(CliHelper.IsCliWhiteSpace);
    }

    #endregion

    #region IExecutionContextBuilder Members

    //public ExecutionContext BuildFromSelf(IReadOnlyDictionary<string, IApp> apps, ReadOnlyMemory<char> input)
    //{
    //    if (apps == null)
    //    {
    //        throw new ArgumentNullException(nameof(apps));
    //    }

    //    input = input.SkipCliWhiteSpaces();

    //    var result = _termExtractor.TryExtract(input.Span, out var appName);
    //    if (result.ErrorCode.HasValue)
    //    {
    //        throw new CliException("App name expected");
    //    }

    //    var app = apps!.GetValueOrDefault(appName);
    //    if (app == null)
    //    {
    //        throw new CliException($"App not found: '{appName}'.");
    //    }

    //    var appInput = input[result.CharsConsumed..].SkipCliWhiteSpaces();

    //    return this.BuildFromApp(app, appInput);
    //}

    public abstract ExecutionContext BuildFromSelf(
        ReadOnlyMemory<char> input,
        bool allowIncomplete);

    public ExecutionContext BuildFromApp(
        IApp app,
        ReadOnlyMemory<char> appInput,
        bool allowIncomplete)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        appInput = appInput.SkipCliWhiteSpaces();

        if (appInput.IsEmpty)
        {
            if (allowIncomplete)
            {
                return new ExecutionContext(
                    app,
                    null,
                    null,
                    null,
                    appInput,
                    null,
                    this.Logger,
                    this.Input,
                    this.Output);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // try get nameless module
        var module = app.GetModule(null);
        if (module != null)
        {
            throw new NotImplementedException();
        }

        // try extract module name
        var result = TermExtractor.TryExtract(appInput.Span, out var moduleName);
        if (result.ErrorCode.HasValue)
        {
            throw new CliException("Module name expected");
        }

        module = app.GetModule(moduleName);
        if (module == null)
        {
            throw new CliException($"Module not found: '{moduleName}'. App is '{app.Name}'.");
        }

        var moduleInput = appInput[result.CharsConsumed..].SkipCliWhiteSpaces();

        return this.BuildFromModule(
            app,
            module,
            moduleInput,
            allowIncomplete);
    }

    public ExecutionContext BuildFromApp(
        IApp app,
        string[] appArgs,
        bool allowIncomplete)
    {
        throw new NotImplementedException();
    }

    public ExecutionContext BuildFromModule(
        IApp app,
        IModule module,
        ReadOnlyMemory<char> moduleInput,
        bool allowIncomplete)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (module == null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        if (!app.Contains(module))
        {
            throw new NotImplementedException();
        }

        moduleInput = moduleInput.SkipCliWhiteSpaces();

        if (moduleInput.IsEmpty)
        {
            if (allowIncomplete)
            {
                return new ExecutionContext(
                    app,
                    module,
                    null,
                    null,
                    moduleInput,
                    null,
                    this.Logger,
                    this.Input,
                    this.Output);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // try get nameless executor
        var executor = module.GetExecutor(null);
        if (executor != null)
        {
            throw new NotImplementedException("todo");
        }

        // try extract executor name
        var result = TermExtractor.TryExtract(moduleInput.Span, out var executorName);
        if (result.ErrorCode.HasValue)
        {
            throw new CliException("Executor name expected");
        }

        executor = module.GetExecutor(executorName);
        if (executor == null)
        {
            // todo: deal with fallback executor

            var moduleName = module.Name ?? module.GetType().FullName;

            throw new CliException($"Executor not found: '{executorName}'. Module is '{moduleName}'. App is '{app.Name}'.");
        }

        if (executor is FallbackExecutor fallbackExecutor)
        {
            fallbackExecutor.Name = executorName!;
        }

        var executorInput = moduleInput[result.CharsConsumed..].SkipCliWhiteSpaces();

        return this.BuildFromExecutor(
            app,
            module,
            executor,
            executorInput,
            allowIncomplete);
    }

    public ExecutionContext BuildFromModule(
        IApp app,
        IModule module,
        string[] moduleArgs,
        bool allowIncomplete)
    {
        throw new NotImplementedException();
    }

    public ExecutionContext BuildFromExecutor(
        IApp app,
        IModule module,
        IExecutor executor,
        ReadOnlyMemory<char> executorInput,
        bool allowIncomplete)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (module == null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        if (executor == null)
        {
            throw new ArgumentNullException(nameof(executor));
        }

        if (!app.Contains(module))
        {
            throw new NotImplementedException();
        }

        if (!module.Contains(executor))
        {
            throw new NotImplementedException();
        }

        return new ExecutionContext(
            app,
            module,
            executor,
            executor.Name,
            executorInput,
            null,
            this.Logger,
            this.Input,
            this.Output);
    }

    public ExecutionContext BuildFromExecutor(
        IApp app,
        IModule module,
        IExecutor executor,
        string[] executorArgs,
        bool allowIncomplete)
    {
        throw new NotImplementedException();
    }

    public ILogger? Logger { get; protected set; }
    public TextReader? Input { get; protected set; }
    public TextWriter? Output { get; protected set; }

    #endregion
}