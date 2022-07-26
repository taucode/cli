namespace TauCode.Cli.Tests.Common.Apps.Curl
{
    public class CurlExecutor : TestExecutorBase
    {
        public CurlExecutor()
            : base(
                null,
                Helper.BuildParsingGraph($".{nameof(CurlExecutor)}.lisp"))
        {
        }
    }
}
