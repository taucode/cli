using Serilog;
using TauCode.Parsing;

// todo clean
namespace TauCode.Cli;

public static class CliExtensions
{
    public static async Task ExecuteAsync(
        this IApp app,
        string appInput,
        ILogger logger,
        TextReader input,
        TextWriter output,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();

        //var success = false;
        //IModule module = null;
        //IExecutor executor = null;

        //do
        //{
        //    var modules = app.Modules;
        //    if (modules.Count != 1)
        //    {
        //        break;
        //    }

        //    module = modules[0];
        //    if (module.Name != null)
        //    {
        //        break;
        //    }

        //    var executors = module.Executors;
        //    if (executors.Count != 1)
        //    {
        //        break;
        //    }

        //    executor = executors[0];
        //    if (executor.Name != null)
        //    {
        //        break;
        //    }

        //    success = true;

        //} while (false);

        //if (!success)
        //{
        //    throw new CliException($"Extension method '{nameof(IApp)}.{nameof(ExecuteAsync)}' is only valid for an app which has single nameless module which in turn has single nameless executor.");
        //}

        //var executionContext = module.CreateExecutionContext(logger, input, output);

        //var tokens = module.Lexer.Tokenize(appInput.AsMemory());
        //await executor.ExecuteAsync(tokens, executionContext, cancellationToken);
    }

    public static async Task ExecuteAsync(
        this IModule module,
        string moduleInput,
        ILogger logger,
        TextReader input,
        TextWriter output,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();

        //var success = false;
        //IExecutor executor = null;

        //do
        //{
        //    var executors = module.Executors;
        //    if (executors.Count != 1)
        //    {
        //        break;
        //    }

        //    executor = executors[0];
        //    if (executor.Name != null)
        //    {
        //        break;
        //    }

        //    success = true;

        //} while (false);

        //if (!success)
        //{
        //    throw new CliException($"Extension method '{nameof(IModule)}.{nameof(ExecuteAsync)}' is only valid for a module which has single nameless executor.");
        //}

        //var executionContext = module.CreateExecutionContext(logger, input, output);

        //var tokens = module.Lexer.Tokenize(moduleInput.AsMemory());
        //await executor.ExecuteAsync(tokens, executionContext, cancellationToken);
    }

    public static async Task ExecuteAsync(
        this IModule module,
        IEnumerable<ILexicalToken> rawTokens,
        ILogger logger,
        TextReader input,
        TextWriter output,
        CancellationToken cancellationToken = default) =>
        await ExecuteAsync(
            module,
            string.Join(" ", rawTokens),
            logger,
            input,
            output,
            cancellationToken);

}