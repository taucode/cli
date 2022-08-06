namespace TauCode.Cli.Tests.Common.Apps.Git.Executors.Checkout;

public class CheckoutExecutor : GitExecutor
{
    public CheckoutExecutor()
        : base(
            "checkout",
            $".{nameof(CheckoutExecutor)}.lisp")
    {
    }
}
