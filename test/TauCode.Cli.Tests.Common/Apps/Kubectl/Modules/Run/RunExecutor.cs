namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Run;

public class RunExecutor : TestExecutorBase
{
    public RunExecutor()
        : base(
            null,
            KubectlHelper.Lexer,
            $".{nameof(RunExecutor)}.lisp")
    {
    }
}