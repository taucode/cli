﻿namespace TauCode.Cli.Tests.Common.Apps.Tau.LibDev.BumpVersion;

public class BumpVersionExecutor : TestExecutorBase
{
    public BumpVersionExecutor()
        : base(
            "bump-version",
            LibDevHelper.Lexer,
            $".{nameof(BumpVersionExecutor)}.lisp")
    {
    }
}
