namespace TauCode.Cli.Tests.Common.Apps.Git.Executors.Branch
{
    public class BranchExecutor : TestExecutorBase
    {
        public BranchExecutor()
            : base(
                "branch",
                GitHelper.BuildParsingGraph($".{nameof(BranchExecutor)}.lisp"))
        {
        }

    }
}
