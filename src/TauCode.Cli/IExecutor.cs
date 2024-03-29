﻿namespace TauCode.Cli;

public interface IExecutor : IDisposable
{
    string? Name { get; }

    void Execute(ExecutionContext executionContext);

    Task ExecuteAsync(ExecutionContext executionContext, CancellationToken cancellationToken = default);
}
