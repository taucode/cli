namespace TauCode.Cli.Tests.Common.Apps.Git;

public abstract class GitExecutor : TestExecutorBase
{
    protected GitExecutor(
        string name,
        string graphResourceName)
        : base(
            name,
            GitHelper.Lexer,
            GitHelper.BuildCliGraph(graphResourceName))
    {
    }
}
