namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Run
{
    public class RunExecutor : TestExecutorBase
    {
        public RunExecutor()
            : base(null, KubectlHelper.BuildParsingGraph($".{nameof(RunExecutor)}.lisp"))
        {
        }
    }
}
