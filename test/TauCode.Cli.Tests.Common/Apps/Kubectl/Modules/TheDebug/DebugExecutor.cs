namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.TheDebug;

public class DebugExecutor : TestExecutorBase
{
    public DebugExecutor()
        : base(
            null,
            KubectlHelper.Lexer,
            $".{nameof(DebugExecutor)}.lisp")
    {
    }
}