﻿using TauCode.Parsing;

namespace TauCode.Cli;

public class Command : IParsingResult
{
    #region ctor

    public Command(string? executorName, ReadOnlyMemory<char> input)
    {
        this.ExecutorName = executorName;
        this.Input = input;
    }

    #endregion

    #region Public

    public string? ExecutorName { get; }

    public ReadOnlyMemory<char> Input { get; }

    public Dictionary<string, List<ILexicalToken>> Arguments { get; } = new();

    public Dictionary<string, List<ILexicalToken>> KeyValues { get; } = new();

    public HashSet<string> Switches { get; } = new();

    #endregion

    #region IParsingResult Members

    public int Version { get; private set; }

    public void IncreaseVersion()
    {
        this.Version++;
    }

    #endregion
}