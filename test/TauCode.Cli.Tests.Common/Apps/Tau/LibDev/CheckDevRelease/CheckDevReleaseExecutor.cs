﻿namespace TauCode.Cli.Tests.Common.Apps.Tau.LibDev.CheckDevRelease
{
    public class CheckDevReleaseExecutor : TestExecutorBase
    {
        public CheckDevReleaseExecutor()
            : base(
                "check-dev-release",
                TauHelper.BuildParsingGraph($".{nameof(CheckDevReleaseExecutor)}.lisp"))
        {
        }
    }
}
