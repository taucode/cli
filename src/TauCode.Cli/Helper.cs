using System;
using System.Text.RegularExpressions;
using TauCode.Cli.Exceptions;

namespace TauCode.Cli
{
    internal static class Helper
    {
        internal static bool IsValidName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            const string pattern = @"^[a-z]([a-z\d]|-[a-z\d])*$";
            return Regex.IsMatch(name, pattern);
        }

        internal static void CheckName(string name, bool canBeNull)
        {
            if (name == null && canBeNull)
            {
                return; // ok
            }

            if (IsValidName(name))
            {
                return; // ok
            }

            throw new CliException($"Invalid name: '{name}'.");
        }
    }
}
