namespace TauCode.Cli.Tests.Common.Apps.Git.Executors.Checkout
{
    public class CheckoutExecutor : TestExecutorBase
    {
        public CheckoutExecutor()
            : base(
                "checkout",
                GitHelper.BuildParsingGraph($".{nameof(CheckoutExecutor)}.lisp"))
        {
        }
    }
}
