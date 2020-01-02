﻿using System;
using System.Linq;
using TauCode.Cli.Nodes;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;

namespace TauCode.Cli
{
    public class CliNodeFactory : NodeFactory
    {
        #region Constructor

        public CliNodeFactory(string nodeFamilyName)
            : base(nodeFamilyName)
        {
        }

        #endregion

        #region Overridden

        public override INode CreateNode(PseudoList item)
        {
            var car = item.GetCarSymbolName();
            INode node;

            switch (car)
            {
                case "SUB-COMMAND":
                    node = this.CreateSubCommandNode(item);
                    break;

                case "KEY-WITH-VALUE":
                    node = this.CreateKeyWithValueNode(item);
                    break;

                case "KEY":
                    node = this.CreateKeyNode(item);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return node;
        }

        #endregion

        #region Node Creators

        private INode CreateSubCommandNode(PseudoList item)
        {
            var value = item.GetSingleKeywordArgument<StringAtom>(":value");
            var name = item.GetItemName();

            INode node = new ExactTextNode(
                value.Value,
                TermTextClass.Instance,
                this.ProcessSubCommand,
                this.NodeFamily,
                name);

            return node;
        }

        private INode CreateKeyNode(PseudoList item)
        {
            var todo = item.ToString();

            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList(); // todo: may throw

            var node = new MultiTextRepresentationNodeLab(
                keyNames,
                new ITextClass[] { KeyTextClass.Instance, },
                this.ProcessKey,
                this.NodeFamily,
                item.GetItemName());

            return node;
        }

        private INode CreateKeyWithValueNode(PseudoList item)
        {
            var keyNames = item
                .GetAllKeywordArguments(":key-names")
                .Select(x => ((StringAtom)x).Value)
                .ToList(); // todo: may throw

            ActionNode keyNameNode = new MultiTextRepresentationNodeLab(
                keyNames,
                new ITextClass[] { KeyTextClass.Instance },
                this.ProcessKeyWithValue,
                this.NodeFamily,
                item.GetItemName());
            INode equalsNode = new ExactPunctuationNode('=', null, this.NodeFamily, null);
            INode choiceNode = this.CreateKeyChoiceNode(item);

            keyNameNode.EstablishLink(equalsNode);
            equalsNode.EstablishLink(choiceNode);

            return keyNameNode;
        }

        private INode CreateKeyChoiceNode(PseudoList item)
        {
            var keyValuesSubform = item.GetSingleKeywordArgument(":key-values");
            if (keyValuesSubform.GetCarSymbolName() != "CHOICE")
            {
                throw new NotImplementedException();
            }

            var classes = keyValuesSubform.GetAllKeywordArguments(":classes").ToList();
            var values = keyValuesSubform.GetAllKeywordArguments(":values").ToList();

            var anyText = values.Count == 1 && values.Single().Equals(Symbol.Create("*"));
            string[] textValues;

            if (anyText)
            {
                textValues = null;
            }
            else
            {
                textValues = values.Select(x => ((StringAtom)x).Value).ToArray(); // todo: try/catch.
            }

            var classTypes = classes.Select(x => this.ParseTextClass(((Symbol)x).Name));

            INode choiceNode = new MultiTextRepresentationNodeLab(
                textValues,
                classTypes,
                ProcessKeyChoice,
                this.NodeFamily,
                null);

            return choiceNode;
        }

        #endregion

        #region Node Actions

        private void ProcessSubCommand(IToken token, IResultAccumulator resultAccumulator)
        {
            var cliCommand = new CliCommand();
            cliCommand.Name = ((TextToken)token).Text;

            resultAccumulator.AddResult(cliCommand);
        }

        private void ProcessKey(IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new CliCommandEntry();
            entry.Key = ((TextToken)token).Text;
            subCommand.Entries.Add(entry);
        }

        private void ProcessKeyWithValue(IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = new CliCommandEntry();
            entry.Key = ((TextToken)token).Text;
            subCommand.Entries.Add(entry);
        }

        private void ProcessKeyChoice(IToken token, IResultAccumulator resultAccumulator)
        {
            var subCommand = resultAccumulator.GetLastResult<CliCommand>();
            var entry = subCommand.Entries.Last();
            entry.Value = ((TextToken)token).Text;
        }

        #endregion

        #region Misc

        private ITextClass ParseTextClass(string textClassSymbolName)
        {
            switch (textClassSymbolName)
            {
                case "STRING":
                    return StringTextClass.Instance;

                case "TERM":
                    return TermTextClass.Instance;

                case "KEY":
                    return KeyTextClass.Instance;

                case "PATH":
                    return PathTextClass.Instance;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
