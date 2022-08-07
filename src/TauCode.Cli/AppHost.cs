using Serilog;

namespace TauCode.Cli;

public class AppHost : ExecutionContextBuilder, IAppHost
{
    #region ctor

    public AppHost(IApp app)
    {
        this.App = app ?? throw new ArgumentNullException(nameof(app));
    }

    #endregion

    #region Protected

    protected virtual void DisposeImpl()
    {
        // idle
    }

    #endregion

    #region Overridden

    public override ExecutionContext BuildFromSelf(ReadOnlyMemory<char> input, bool allowIncomplete)
    {
        throw new NotSupportedException();
    }

    public override ILogger? GetLogger() => this.Logger;

    public override TextReader? GetInput() => this.Input;

    public override TextWriter? GetOutput() => this.Output;

    #endregion

    #region IAppHost Members

    public IApp App { get; }

    public ILogger? Logger { get; set; }

    public TextReader? Input { get; set; }

    public TextWriter? Output { get; set; }

    public void Run(ReadOnlyMemory<char> input)
    {
        var context = this.BuildFromApp(this.App, input, false);
        context.Executor!.Execute(context);
    }

    public async Task RunAsync(ReadOnlyMemory<char> input, CancellationToken cancellationToken = default)
    {
        var context = this.BuildFromApp(this.App, input, false);
        await context.Executor!.ExecuteAsync(context, cancellationToken);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        this.App.Dispose();
        this.DisposeImpl();
    }

    #endregion
}