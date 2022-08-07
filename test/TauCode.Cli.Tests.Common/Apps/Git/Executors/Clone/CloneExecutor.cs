namespace TauCode.Cli.Tests.Common.Apps.Git.Executors.Clone;

public class CloneExecutor : GitExecutor
{
    public CloneExecutor()
        : base(
            "clone",
            $".{nameof(CloneExecutor)}.lisp")
    {
    }
}
