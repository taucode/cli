namespace TauCode.Cli.Tests.Common.Apps.Git.Executors.Branch;

public class BranchExecutor : GitExecutor
{
    public BranchExecutor()
        : base(
            "branch",
            $".{nameof(BranchExecutor)}.lisp")
    {
    }
}
