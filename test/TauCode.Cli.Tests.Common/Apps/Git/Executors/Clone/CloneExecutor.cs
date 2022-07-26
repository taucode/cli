namespace TauCode.Cli.Tests.Common.Apps.Git.Executors.Clone
{
    public class CloneExecutor : TestExecutorBase
    {
        public CloneExecutor()
            : base(
                "clone",
                GitHelper.BuildParsingGraph($".{nameof(CloneExecutor)}.lisp"))
        {
        }
    }
}
