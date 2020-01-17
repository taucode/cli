﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    // todo clean up
    // todo checks for all extensions when using LINQ.
    public static class CliExtensions
    {
        private class CatchAllAndThrowNode : ActionNode
        {
            public CatchAllAndThrowNode(Action<IToken> handler, INodeFamily family, string name)
                : base(BuildAction(handler), family, name)
            {
            }

            private static Action<ActionNode, IToken, IResultAccumulator> BuildAction(Action<IToken> handler)
            {
                void Result(ActionNode dummyActionNode, IToken token, IResultAccumulator resultAccumulator)
                {
                    handler(token);

                    throw new NotImplementedException("You wasn't expected to get here!");
                }

                return Result;
            }

            protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator) => true;
        }

        public static ICliFunctionalityProvider AddCustomHandlerWithParameter(
            this ICliFunctionalityProvider functionalityProvider,
            Action<IToken> handler,
            params string[] texts)
        {
            // todo: a lot of copy/paste (see AddCustomHandler method)
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Name == null)
            {
                throw new CliException("Cannot add custom handler to a nameless functionality.");
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            if (texts.Length == 0)
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot be empty.");
            }

            var tokens = new List<TextToken>();
            ITextClass textClass;

            try
            {
                ILexer lexer = new CliLexer();

                foreach (var text in texts)
                {
                    var singleTextTokens = lexer.Lexize(text);
                    var isValid =
                        singleTextTokens.Count == 1 &&
                        singleTextTokens.Single() is TextToken;

                    if (!isValid)
                    {
                        throw new NotImplementedException(); // error.
                    }

                    var token = (TextToken)singleTextTokens.Single();
                    tokens.Add(token);
                }

                var classes = tokens.Select(x => x.Class).Distinct().ToList();
                if (classes.Count > 1)
                {
                    throw new NotImplementedException(); // error.
                }

                textClass = classes.Single();

                var decorations = tokens.Select(x => x.Decoration).Distinct().ToList();
                if (decorations.Count != 1)
                {
                    throw new NotImplementedException(); // error.
                }

                if (decorations.Single() != NoneTextDecoration.Instance)
                {
                    throw new NotImplementedException(); // error.
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Bad texts. todo", ex);
            }

            INodeFamily nodeFamily = new NodeFamily("dummy"); // todo

            var commandNode = new MultiTextNode(
                tokens.Select(x => x.Text),
                new[] { textClass },
                true,
                null,
                nodeFamily,
                null);

            var argumentNode = new CatchAllAndThrowNode(handler, nodeFamily, null);

            functionalityProvider.Node.EstablishLink(commandNode);
            commandNode.EstablishLink(argumentNode);

            return functionalityProvider;
        }

        public static ICliFunctionalityProvider AddCustomHandler(
            this ICliFunctionalityProvider functionalityProvider,
            Action action,
            params string[] texts)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Name == null)
            {
                throw new CliException("Cannot add custom handler to a nameless functionality.");
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (texts == null)
            {
                throw new ArgumentNullException(nameof(texts));
            }

            if (texts.Length == 0)
            {
                throw new ArgumentException($"'{nameof(texts)}' cannot be empty.");
            }

            var tokens = new List<TextToken>();
            ITextClass textClass;

            try
            {
                ILexer lexer = new CliLexer();

                foreach (var text in texts)
                {
                    var singleTextTokens = lexer.Lexize(text);
                    var isValid =
                        singleTextTokens.Count == 1 &&
                        singleTextTokens.Single() is TextToken;

                    if (!isValid)
                    {
                        throw new NotImplementedException(); // error.
                    }

                    var token = (TextToken)singleTextTokens.Single();
                    tokens.Add(token);
                }

                var classes = tokens.Select(x => x.Class).Distinct().ToList();
                if (classes.Count > 1)
                {
                    throw new NotImplementedException(); // error.
                }

                textClass = classes.Single();

                var decorations = tokens.Select(x => x.Decoration).Distinct().ToList();
                if (decorations.Count != 1)
                {
                    throw new NotImplementedException(); // error.
                }

                if (decorations.Single() != NoneTextDecoration.Instance)
                {
                    throw new NotImplementedException(); // error.
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("Bad texts. todo", ex);
            }

            INodeFamily nodeFamily = new NodeFamily("dummy"); // todo

            var node = new MultiTextNode(
                tokens.Select(x => x.Text),
                new[] { textClass },
                true,
                (actionNode, token, resultAccumulator) =>
                {
                    action();
                    throw new CliCustomHandlerException();
                },
                nodeFamily,
                null);

            functionalityProvider.Node.EstablishLink(node);
            return functionalityProvider;
        }

        public static ICliFunctionalityProvider AddVersion(this ICliFunctionalityProvider functionalityProvider)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (functionalityProvider.Version == null)
            {
                throw new ArgumentException(
                    $"Functionality provider '{functionalityProvider.Name}' doesn't support version.",
                    nameof(functionalityProvider));
            }

            return functionalityProvider.AddCustomHandler(
                () => functionalityProvider.Output.WriteLine(functionalityProvider.Version),
                "--version");
        }

        public static ICliFunctionalityProvider AddHelp(this ICliFunctionalityProvider functionalityProvider)
        {
            if (functionalityProvider == null)
            {
                throw new ArgumentNullException(nameof(functionalityProvider));
            }

            if (!functionalityProvider.SupportsHelp)
            {
                throw new ArgumentException(
                    $"Functionality provider '{functionalityProvider.Name}' doesn't support help.",
                    nameof(functionalityProvider));
            }

            return functionalityProvider.AddCustomHandler(
                () => functionalityProvider.Output.WriteLine(functionalityProvider.GetHelp()),
                "--help");
        }

        public static CliCommandEntry GetSingleOrDefaultEntryByAlias(
            this IEnumerable<CliCommandEntry> entries,
            string alias)
        {
            // todo checks
            // todo can throw
            return entries.SingleOrDefault(x =>
                string.Equals(alias, x.Alias, StringComparison.InvariantCultureIgnoreCase));
        }

        public static CliCommandEntry GetSingleEntryByAlias(this IEnumerable<CliCommandEntry> entries, string alias)
        {
            // todo checks
            // todo can throw
            return entries.Single(x => string.Equals(alias, x.Alias, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IList<CliCommandEntry> GetEntriesByAlias(this IEnumerable<CliCommandEntry> entries, string alias)
        {
            // todo checks
            // todo can throw
            return entries
                .Where(x => string.Equals(alias, x.Alias, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public static bool ContainsOption(this IEnumerable<CliCommandEntry> entries, string optionAlias)
        {
            var option = entries.SingleOrDefault(x =>
                x.Kind == CliCommandEntryKind.Option &&
                string.Equals(x.Alias, optionAlias, StringComparison.InvariantCultureIgnoreCase));

            return option != null;
        }

        public static string GetArgument(this IEnumerable<CliCommandEntry> entries, string argumentAlias)
        {
            var entry = entries.Single(x =>
                x.Kind == CliCommandEntryKind.Argument &&
                string.Equals(x.Alias, argumentAlias, StringComparison.InvariantCultureIgnoreCase));

            return entry.Value;
        }

        public static string GetSingleKeyValue(this IEnumerable<CliCommandEntry> entries, string keyAlias)
        {
            var entry = entries.Single(x =>
                x.Kind == CliCommandEntryKind.KeyValuePair &&
                string.Equals(x.Alias, keyAlias, StringComparison.InvariantCultureIgnoreCase));

            return entry.Value;
        }

        public static string[] GetKeyValues(this IEnumerable<CliCommandEntry> entries, string keyAlias)
        {
            return entries
                .Where(x =>
                    x.Kind == CliCommandEntryKind.KeyValuePair &&
                    string.Equals(x.Alias, keyAlias, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .ToArray();
        }

        public static string[] GetAllOptionAliases(this IEnumerable<CliCommandEntry> entries)
        {
            return entries
                .Where(x => x.Kind == CliCommandEntryKind.Option)
                .Select(x => x.Alias)
                .ToArray();
        }

        public static CliCommand AddAddInCommand(this IResultAccumulator resultAccumulator, string addInName)
        {
            if (resultAccumulator.Count != 0)
            {
                throw new NotImplementedException(); // internal error - accumulator should be empty.
            }

            var command = CliCommand.CreateAddInCommand(addInName);
            resultAccumulator.AddResult(command);

            return command;
        }

        public static CliCommand EnsureWorkerCommand(this IResultAccumulator resultAccumulator, string workerName)
        {
            // todo check on resultAccumulator

            if (resultAccumulator.Count == 0)
            {
                throw new NotImplementedException();
            }
            else
            {
                var command = resultAccumulator.GetLastResult<CliCommand>();
                //if (command.AddInName == null)
                //{
                //    throw new NotImplementedException(); // error - shouldn't be
                //}

                command.SetWorkerName(workerName);
                return command;
            }
        }

        public static CliCommand EnsureWorkerCommand(this IResultAccumulator resultAccumulator)
        {
            if (resultAccumulator == null)
            {
                throw new ArgumentNullException(nameof(resultAccumulator));
            }

            if (resultAccumulator.Count == 1)
            {
                return resultAccumulator.GetLastResult<CliCommand>();
            }

            var command = CliCommand.CreateNamelessWorkerCommand();
            return command;
        }

        //public static CliCommand EnsureAddInCommand(
        //    this IResultAccumulator resultAccumulator,
        //    string addInName)
        //{
        //    CliCommand command;

        //    if (resultAccumulator.Count == 0)
        //    {
        //        command = new CliCommand
        //        {
        //            AddInName = addInName,
        //        };
        //        resultAccumulator.AddResult(command);
        //    }
        //    else
        //    {
        //        command = resultAccumulator.GetLastResult<CliCommand>();

        //        if (command.AddInName != addInName)
        //        {
        //            throw new NotImplementedException(); // todo wat?
        //        }
        //    }

        //    return command;
        //}

        //public static CliCommand EnsureWorkerCommand(
        //    this IResultAccumulator resultAccumulator,
        //    string workerName)
        //{
        //    CliCommand command;
        //    //var workerName = workerNode.Properties.GetOrDefault("worker-name");

        //    if (resultAccumulator.Count == 0)
        //    {
        //        command = new CliCommand
        //        {
        //            WorkerName = workerName,
        //        };
        //        resultAccumulator.AddResult(command);
        //    }
        //    else
        //    {
        //        command = resultAccumulator.GetLastResult<CliCommand>();
        //        if (command.WorkerName == null)
        //        {
        //            command.WorkerName = workerName;
        //        }
        //        else if (command.WorkerName != workerName)
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }

        //    return command;
        //}

        public static CliCommand ParseLine(this ICliHost host, string line)
        {
            // todo checks
            return host.ParseCommand(new[] { line });
        }
    }
}
