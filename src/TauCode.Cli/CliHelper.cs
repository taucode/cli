using System;
using System.Text.RegularExpressions;
using TauCode.Extensions;

namespace TauCode.Cli
{
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
    }
}
