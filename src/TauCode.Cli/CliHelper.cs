﻿using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TauCode.Cli.Extractors;
using TauCode.Extensions;

namespace TauCode.Cli;

// todo sweep out not used methods
public static class CliHelper
{
    internal static bool IsDecimalDigit(this char c)
    {
        if (c >= '0' && c <= '9')
        {
            return true;
        }

        return false;
    }

    internal static bool IsLatinLetterInternal(this char c)
    {
        if (c >= 'a' && c <= 'z')
        {
            return true;
        }

        if (c >= 'A' && c <= 'Z')
        {
            return true;
        }

        return false;
    }

    internal static bool IsInlineWhiteSpaceOrCaretControl(this char c) => IsInlineWhiteSpace(c) || IsCaretControl(c);

    internal static bool IsCaretControl(this char c) => c.IsIn('\r', '\n');

    internal static bool IsInlineWhiteSpace(this char c) => c.IsIn(' ', '\t');

    public static bool IsValidKey(string key)
    {
        const string pattern = @"^-{1,2}[a-zA-Z]([a-zA-Z\d]|-[a-zA-Z\d])*$";
        return Regex.IsMatch(key, pattern);
    }

    public static void AddExecutors(this IModule module, params IExecutor[] executors)
    {
        foreach (var executor in executors)
        {
            module.AddExecutor(executor);
        }
    }

    public static void AddModules(this IApp app, params IModule[] modules)
    {
        foreach (var module in modules)
        {
            app.AddModule(module);
        }
    }

    // todo not nice. use proper Extractor?
    public static bool IsValidTerm(string term)
    {
        if (term == null)
        {
            throw new ArgumentNullException(nameof(term));
        }

        if (term.Length == 0)
        {
            return false;
        }

        return Regex.IsMatch(term, @"^[a-z]([a-z\d]|-[a-z\d])*$");
    }

    // todo rename
    public static bool IsCliWhiteSpace(ReadOnlySpan<char> input, int pos)
    {
        var c = input[pos];
        return c.IsIn('\t', '\n', '\v', '\f', '\r', ' ');
    }

    internal static ReadOnlyMemory<char> SkipCliWhiteSpaces(this ReadOnlyMemory<char> input)
    {
        var pos = 0;
        var span = input.Span;
        while (true)
        {
            if (pos == span.Length)
            {
                break;
            }

            if (IsCliWhiteSpace(span, pos))
            {
                // go on
            }
            else
            {
                break;
            }

            pos++;
        }

        input = input[pos..];
        return input;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetCommandLine();

    private static readonly WinCmdStringExtractor StringExtractor = new();

    public static ReadOnlyMemory<char> GetCommandLineArguments()
    {
        var ptr = GetCommandLine();
        var commandLine = Marshal.PtrToStringAuto(ptr);

        if (commandLine == null)
        {
            return null;
        }

        var span = commandLine.AsSpan();

        var extractionResult = StringExtractor.TryExtract(span, out var executable);
        if (extractionResult.ErrorCode.HasValue)
        {
            return null;
        }

        var result = commandLine.AsMemory();
        result = result[extractionResult.CharsConsumed..];
        return result;
    }
}
