namespace TauCode.Cli.Tests.Common.Apps.Kubectl.Modules.Create
{
    public class CreateExecutor : TestExecutorBase
    {
        public CreateExecutor()
            : base(null, KubectlHelper.BuildParsingGraph($".{nameof(CreateExecutor)}.lisp"))
        {
        }
    }
}
