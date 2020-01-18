﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    // todo: protected virtual CliNodeFactory CreateNodeFactory, for tunability.
    // todo clean up
    public class CliWorkerNodeFactory : NodeFactoryBase
    {
        private readonly IDictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>> _fallbackPredicates;

        public CliWorkerNodeFactory(
            string nodeFamilyName,
            IDictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>> fallbackPredicates = null)
            : base(
                nodeFamilyName,
                new List<ITextClass>
                {
                    IntegerTextClass.Instance,
                    KeyTextClass.Instance,
                    PathTextClass.Instance,
                    TermTextClass.Instance,
                    StringTextClass.Instance,
                    UrlTextClass.Instance,
                },
                true)
        {
            _fallbackPredicates = fallbackPredicates != null ?
                fallbackPredicates.ToDictionary(x => x.Key.ToLowerInvariant(), x => x.Value) :
                new Dictionary<string, Func<FallbackNode, IToken, IResultAccumulator, bool>>();
        }

        // todo
        //protected override Func<FallbackNode, IToken, IResultAccumulator, bool> CreateFallbackPredicate(string nodeName)
        //{
        //    if (nodeName.ToLowerInvariant() == "bad-key-fallback")
        //    {
        //        return BadKeyFallbackPredictate;
        //    }

        //    throw new ArgumentException($"Unknown fallback name: '{nodeName}'.", nameof(nodeName));
        //}

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName().ToLowerInvariant();
            if (car == "worker")
            {
                INode workerNode;

                var workerName = item.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
                if (workerName == null)
                {
                    workerNode = new IdleNode(this.NodeFamily, "todo: Unnamed worker node");
                }
                else
                {
                    workerNode = new MultiTextNode(
                        item
                            .GetAllKeywordArguments(":verbs")
                            .Cast<StringAtom>()
                            .Select(x => x.Value)
                            .ToList(),
                        new ITextClass[]
                        {
                            TermTextClass.Instance,
                        },
                        true,
                        WorkerAction,
                        this.NodeFamily,
                        $"Worker Node. Name: [{workerName}]");

                    workerNode.Properties["worker-name"] = workerName;
                }

                return workerNode;
            }

            var node = base.CreateNode(item);
            if (node is FallbackNode)
            {
                return node;
            }

            if (!(node is ActionNode))
            {
                throw new NotImplementedException(); // todo
            }

            var baseResult = (ActionNode)node;

            if (baseResult == null)
            {
                throw new CliException($"Could not build node for item '{car}'.");
            }

            var action = item.GetSingleKeywordArgument<Symbol>(":action", true)?.Name?.ToLowerInvariant();
            string alias;

            switch (action)
            {
                case "key":
                    baseResult.Action = KeyAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;

                case "value":
                    baseResult.Action = ValueAction;
                    break;

                case "option":
                    baseResult.Action = OptionAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;

                case "argument":
                    baseResult.Action = ArgumentAction;
                    alias = item.GetSingleKeywordArgument<Symbol>(":alias").Name;
                    baseResult.Properties["alias"] = alias;
                    break;


                default:
                    throw new CliException($"Keyword ':action' is missing for item '{car}'.");
            }

            return baseResult;
        }

        protected override Func<FallbackNode, IToken, IResultAccumulator, bool> CreateFallbackPredicate(string nodeName)
        {
            return _fallbackPredicates.GetOrDefault(nodeName.ToLowerInvariant()) ?? throw new NotImplementedException(); // todo nu such predicate
        }

        private void WorkerAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            resultAccumulator.EnsureWorkerCommand(node.Properties["worker-name"]);

            //if (resultAccumulator.Count == 0)
            //{
            //    var command = new CliCommand
            //    {
            //        WorkerName = node.Properties["worker-name"],
            //    };

            //    resultAccumulator.AddResult(command);
            //}
            //else
            //{
            //    var command = resultAccumulator.GetLastResult<CliCommand>();
            //    command.WorkerName = node.Properties["worker-name"];
            //}
        }

        private void KeyAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.EnsureWorkerCommand();

            //var command = resultAccumulator.GetLastResult<CliCommand>();
            //var entry = new CliCommandEntry
            //{
            //    Alias = node.Properties["alias"],
            //};
            var alias = node.Properties["alias"];
            var key = TokenToKey(token);

            var entry = CliCommandEntry.CreateKeyValuePair(alias, key);
            command.Entries.Add(entry);
        }

        private static string TokenToKey(IToken token)
        {
            // todo checks?
            var textToken = (TextToken)token;
            if (textToken.Class is KeyTextClass)
            {
                return textToken.Text;
            }

            throw new NotImplementedException(); // error
        }

        private void ValueAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();
            var entry = command.Entries.Last();
            var textToken = (TextToken)token;
            //entry.Value = textToken.Text;
            entry.SetKeyValue(textToken.Text);
        }

        private void OptionAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = resultAccumulator.GetLastResult<CliCommand>();

            var alias = node.Properties["alias"];
            var key = TokenToKey(token);

            var entry = CliCommandEntry.CreateOption(alias, key);
            command.Entries.Add(entry);
        }

        private void ArgumentAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            // todo: EnsureCommand (uses command from 'IResultAccumulator resultAccumulator', or adds new; everywhere!)
            //CliCommand command;
            //if (resultAccumulator.Count == 0)
            //{
            //    command = new CliCommand
            //    {
            //        WorkerName = node.Properties.GetOrDefault("worker-name"),
            //    };

            //    resultAccumulator.AddResult(command);
            //}
            //else
            //{
            //    command = resultAccumulator.GetLastResult<CliCommand>();
            //    command.WorkerName = node.Properties.GetOrDefault("worker-name");
            //}

            //var command = resultAccumulator.GetLastResult<CliCommand>();

            var command = resultAccumulator.EnsureWorkerCommand();

            var alias = node.Properties["alias"];
            var argument = TokenToArgument(token);
            var entry = CliCommandEntry.CreateArgument(alias, argument);
            command.Entries.Add(entry);
        }

        private string TokenToArgument(IToken token)
        {
            // todo checks?
            var textToken = (TextToken)token;
            return textToken.Text;
        }
    }
}