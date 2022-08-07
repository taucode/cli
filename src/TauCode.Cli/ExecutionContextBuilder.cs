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

    protected ExecutionContextBuilder()
    {
        TermExtractor = new TermExtractor(CliHelper.IsCliWhiteSpace);
    }

    #endregion

    #region IExecutionContextBuilder Members

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
                    this.GetLogger(),
                    this.GetInput(),
                    this.GetOutput());
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        // try get nameless module
        var module = app.GetModule(null);
        ReadOnlyMemory<char> moduleInput;

        if (module == null)
        {
            // there is no nameless module

            // try extract module name
            var result = TermExtractor.TryExtract(appInput.Span, out var moduleName);
            if (result.ErrorCode.HasValue)
            {
                throw new CliException("Module name expected.");
            }

            module = app.GetModule(moduleName);
            if (module == null)
            {
                throw new CliException($"Module not found: '{moduleName}'. App is '{app.Name}'.");
            }

            moduleInput = appInput[result.CharsConsumed..].SkipCliWhiteSpaces();
        }
        else
        {
            moduleInput = appInput;
        }

        return this.BuildFromModule(
            app,
            module,
            moduleInput,
            allowIncomplete);
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
            throw new NotImplementedException("error");
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
                    this.GetLogger(),
                    this.GetInput(),
                    this.GetOutput());
            }
            else
            {
                throw new NotImplementedException("error");
            }
        }

        // try get nameless executor
        var executor = module.GetExecutor(null);
        ReadOnlyMemory<char> executorInput;

        if (executor == null)
        {
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

            executorInput = moduleInput[result.CharsConsumed..].SkipCliWhiteSpaces();
        }
        else
        {
            executorInput = moduleInput;
        }

        return this.BuildFromExecutor(
            app,
            module,
            executor,
            executorInput,
            allowIncomplete);
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
            this.GetLogger(),
            this.GetInput(),
            this.GetOutput());
    }

    public abstract ILogger? GetLogger();

    public abstract TextReader? GetInput();

    public abstract TextWriter? GetOutput();

    #endregion
}